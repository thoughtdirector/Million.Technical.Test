namespace Million.Technical.Test.Application.Commands.Validations
{
    public interface ICreateOwnerValidator
    {
        void ValidateAndThrow(CreateOwnerCommand command);
    }
}