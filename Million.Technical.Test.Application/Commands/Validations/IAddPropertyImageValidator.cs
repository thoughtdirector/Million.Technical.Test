namespace Million.Technical.Test.Application.Commands.Validations
{
    public interface IAddPropertyImageValidator
    {
        void ValidateAndThrow(AddPropertyImageCommand command);
    }
}