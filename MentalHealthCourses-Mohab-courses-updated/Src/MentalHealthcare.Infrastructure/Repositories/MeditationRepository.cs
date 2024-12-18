using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using MentalHealthcare.Infrastructure.Persistence;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Infrastructure.Repositories
{
    public class MeditationRepository(
        MentalHealthDbContext dbContext
       , ILogger<MeditationRepository> logger) : IMeditationRepository
    {


        /// <summary>
        /// Adds a new meditation and saves it to the database.
        /// </summary>
        /// <param name="meditation">The meditation entity to add.</param>
        /// <returns>The ID of the added meditation.</returns>
        /// <exception cref="DbUpdateException">Thrown when there is an error updating the database, including foreign key violations.</exception>
        /// <exception cref="Exception">Thrown when there is a general error.</exception>
        public async Task<int> AddMeditationAsync(Meditation meditation)
        {
            try
            {
                await dbContext.Meditations.AddAsync(meditation);
                await dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlException)
            {
                // TODO: Check for specific foreign key constraint violations
                foreach (SqlError error in sqlException.Errors)
                {
                    // TODO: SQL Server foreign key violation error number
                    if (error.Number == 547) // Foreign key violation error number
                    {
                        logger.LogError(ex, "Foreign key violation: {Message}", error.Message);
                        throw new ResourceNotFound(nameof(Meditation), meditation.MeditationId.ToString());
                    }
                }

                // TODO: Log error details for further investigation
                logger.LogError(ex, "An error occurred while saving the Meditation.");
                throw;
            }
            catch (Exception ex)
            {
                // TODO: Log general exceptions for monitoring and debugging
                logger.LogError(ex, "An error occurred while saving the Meditation.");
                throw;
            }

            return meditation.MeditationId;
        }

        /// <summary>
        /// Deletes a meditation from the database.
        /// </summary>
        /// <param name="meditation">The meditation entity to delete.</param>
        /// <returns>A success message or failure message.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the specified meditation does not exist.</exception>
        public async Task DeleteMeditationAsync(int Meditation_ID)
        {
            try
            {
                var Selected_Meditation = await dbContext.Meditations.FindAsync(Meditation_ID);
                if (Selected_Meditation == null)
                { throw new ResourceNotFound(nameof(Meditation), Meditation_ID.ToString()); }
                dbContext.Meditations.Remove(Selected_Meditation); await dbContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw new Exception($"Failed Process! Error: {ex.Message}", ex);
            }


        }
        /// <summary>
        /// Retrieves a paginated and sorted list of meditations based on search criteria. 
        ///  </summary> 
        /// <param name="searchName">The search term to filter meditations by title.</param>
        /// <param name="pageNumber">The page number for pagination.</param> 
        ///<param name="pageSize">The number of meditations per page.</param> 
        ///<param name="sortBy">The field to sort the meditations by (e.g., "title", "date").</param> 
        ///<returns>A tuple containing the total count of meditations and the list of meditations for the specified page.</returns>
        public async Task<(int, IEnumerable<Meditation>)> GetAllMeditationsAsync(string? searchName, int pageNumber, int pageSize, string? sortBy)
        {

            searchName ??= string.Empty;
            searchName = searchName.ToLower();
            var baseQuery = dbContext.Meditations
                .Where(r => r.Title.ToLower().Contains(searchName));










            //TODO : Apply sorting
            baseQuery = sortBy switch
            {
                "title" => baseQuery.OrderBy(r => r.Title),// Default sorting
                "date" => baseQuery.OrderBy(r => r.CreatedDate),
                _ => baseQuery.OrderBy(r => r.Title)
            };


            var totalCount = await baseQuery.CountAsync();
            var Meditation = await baseQuery
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();
            return (totalCount, Meditation);







        }
        /// <summary> 
        /// Retrieves a meditation by its ID. 
        /// </summary> /// <param name="meditationId">The ID of the meditation to retrieve.
        /// </param> 
        ///<returns>The meditation that matches the given ID.</returns> 
        ///<exception cref="ResourceNotFound">Thrown when the meditation is not found.</exception>

        public async Task<Meditation> GetMeditationsById(int MeditationId)
        {

            var Selected_Meditation = await dbContext.Meditations
                .Where(A => A.MeditationId == MeditationId)
                .Select(A => new Meditation
                {
                    MeditationId = A.MeditationId,
                    Title = A.Title,
                    UploadedBy = A.UploadedBy,
                    UploadedById = A.UploadedById,
                    Content = A.Content,
                    CreatedDate = A.CreatedDate,

                }).FirstOrDefaultAsync();

            if (Selected_Meditation == null)
            {
                throw new ResourceNotFound(nameof(Meditation), MeditationId.ToString());
            }

            return Selected_Meditation;
        }
        /// <summary>
        /// Checks if a meditation with the given title exists. 
        /// </summary> /// <param name="title">The title of the meditation to check for existence.</param> 
        /// <returns>True if a meditation with the given title exists, otherwise false.</returns>
        public async Task<bool> IsExistByTitle(string title)
        {
            return await dbContext.Meditations
           .Where(a => a.Title.Equals(title))
           .FirstOrDefaultAsync() != null;
        }
        /// <summary>
        /// Updates an existing meditation.
        /// </summary>
        /// <param name="meditation">The meditation entity with updated information.</param>
        /// <returns>A success message indicating the meditation has been updated.</returns>
        /// <exception cref="ArgumentException">Thrown when the meditation entity is null.</exception>
        /// <exception cref="DbUpdateException">Thrown when the update operation fails.</exception>
        public async Task<string> UpdateMeditationAsync(Meditation meditation)
        {
            // TODO: Validate the meditation parameter
            if (meditation == null)
            {
                throw new ArgumentNullException(nameof(meditation), "Meditation must not be null.");
            }

            // TODO: Add logging to capture the start of the update operation
            logger.LogInformation("Starting update operation for Meditation ID: {MeditationId}", meditation.MeditationId);

            try
            {
                dbContext.Meditations.Update(meditation);
                await dbContext.SaveChangesAsync(); // Ensure changes are saved to the database

                // TODO: Log the successful update operation
                logger.LogInformation("Meditation updated successfully with ID: {MeditationId}", meditation.MeditationId);
                return "The Meditation has been updated successfully!";
            }
            catch (DbUpdateException ex)
            {
                // TODO: Log the exception details for debugging and monitoring
                logger.LogError(ex, "An error occurred while updating the Meditation with ID: {MeditationId}", meditation.MeditationId);
                throw new DbUpdateException("An error occurred while updating the meditation!", ex);
            }
        }



        public async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }

    }












}
