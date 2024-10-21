using FluentValidation;
using MediatR;

namespace MentalHealthcare.Application.Common
{
    public class Validation_Behaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
       where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _add_Articles_Validator;

        public Validation_Behaviour(IEnumerable<IValidator<TRequest>> Add_Articles_Validator)
        {
            _add_Articles_Validator = Add_Articles_Validator;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_add_Articles_Validator.Any())
            {
                var context = new ValidationContext<TRequest>(request);
                //To Do : Check The verification of Validators 
                var validationResults = await Task.WhenAll(_add_Articles_Validator.Select(v => v.ValidateAsync(context, cancellationToken)));
                var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

                //To Do : Check if one of Validators not verificate

                if (failures.Count != 0)
                {
                    var message = failures.Select(x => x.ErrorMessage).FirstOrDefault();

                    throw new ValidationException(message);

                }
            }
            //To Do : All Validators verificate

            return await next();
        }



    }
}


