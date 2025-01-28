namespace Million.Technical.Test.Application.Commands.Validations
{
    public interface ICreatePropertyValidator
    {
        void ValidateAndThrow(CreatePropertyCommand command);
    }
}