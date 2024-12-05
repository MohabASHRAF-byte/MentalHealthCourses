using System.Collections.Concurrent;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Application.Background_Services;

public class OtpService(
    ILogger<OtpService> logger
    ) : IHostedService, IDisposable
{
    private Timer? _cleanupTimer;
    private const int CleanupInterval = 10;
    private const int ExpirtyTime = 50;

    private static readonly ConcurrentDictionary<(string,string), (string Otp, DateTime Expiry)> _otpStore =
        new ();

    /// <summary>
    /// Retrieves an OTP if it exists and hasn't expired, otherwise returns null.
    /// </summary>
    public string? GetOtp(string key, string tenant)
    {
        // Attempt to get the OTP information from the store
        if (_otpStore.TryGetValue((key, tenant), out var otpInfo) && otpInfo.Expiry > DateTime.UtcNow)
        {
            return otpInfo.Otp; // OTP is still valid
        }
    
        // If the OTP is not found or expired, remove it if it exists
        if (_otpStore.TryGetValue((key, tenant), out otpInfo))
        {
            _otpStore.Remove((key, tenant), out _);
            logger.LogInformation("Expired OTP removed for key: {Key}", key);
        }
    
        // No valid OTP found
        return null;
    }



    /// <summary>
    /// Adds or updates an OTP, setting its expiry to 5 minutes from now.
    /// </summary>


    public void AddOrUpdateOtp(string key,string tenant, string otp)
    {
        var expiryTime = DateTime.UtcNow.AddMinutes(ExpirtyTime); // OTP expires in 5 minutes
        _otpStore.AddOrUpdate((key,tenant), (otp, expiryTime), (existingKey, existingValue) => (otp, expiryTime));
        logger.LogInformation("OTP added/updated for key: {Key}", key);
    }

    /// <summary>
    /// Starts the background cleanup process that removes expired OTPs every 2 minutes.
    /// </summary>
    public Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("OTP Service is starting.");
        // Start the timer to clean up every 2 minutes
        _cleanupTimer = new Timer(RemoveExpiredOtps, null, TimeSpan.Zero, TimeSpan.FromMinutes(CleanupInterval));
        return Task.CompletedTask;
    }

    /// <summary>
    /// Stops the background cleanup process.
    /// </summary>
    public Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("OTP Service is stopping.");
        _cleanupTimer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Periodically removes expired OTPs.
    /// </summary>
    private void RemoveExpiredOtps(object? state)
    {
        var expiredKeys = _otpStore.Where(x => x.Value.Expiry < DateTime.UtcNow).Select(x => x.Key).ToList();
        foreach (var key in expiredKeys)
        {
            _otpStore.Remove(key, out _);
            logger.LogInformation("Removed expired OTP for key: {Key}", key);
        }
    }

    public void Dispose()
    {
        _cleanupTimer?.Dispose();
    }
}