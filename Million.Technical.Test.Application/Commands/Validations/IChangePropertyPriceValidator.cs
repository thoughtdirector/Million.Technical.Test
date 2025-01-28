namespace Million.Technical.Test.Application.Commands.Validations
{
    public interface IChangePropertyPriceValidator
    {
        void ValidateAndThrow(ChangePropertyPriceCommand command);
    }
}