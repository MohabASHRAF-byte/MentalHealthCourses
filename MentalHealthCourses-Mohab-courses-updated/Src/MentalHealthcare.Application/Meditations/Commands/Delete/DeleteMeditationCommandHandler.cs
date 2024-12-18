using MediatR;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Meditations.Command.Delete
{
    public class DeleteMeditationCommandHandler
        (IMeditationRepository _meditationRepository,
            ILogger<DeleteMeditationCommandHandler> _logger) : IRequestHandler<DeleteMeditationCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteMeditationCommand request, CancellationToken cancellationToken)
        {
            // TODO: Retrieve the meditation entity by ID
            var meditation = await _meditationRepository.GetMeditationsById(request.MeditationId);

            if (meditation == null)
            {
                _logger.LogWarning("Meditation with ID: {MeditationId} and Title: {Title} not found.", request.MeditationId, request.MeditationId);
                return Unit.Value;
            }

            // TODO: Delete the meditation
            await _meditationRepository.DeleteMeditationAsync(meditation.MeditationId);
            _logger.LogInformation("Meditation deleted successfully with ID: {MeditationId} and Title: {Title}", request.MeditationId, request.MeditationId);

            return Unit.Value; // Indicate successful completion







        }
    }
}