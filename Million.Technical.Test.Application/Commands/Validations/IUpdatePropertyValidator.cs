namespace Million.Technical.Test.Application.Commands.Validations
{
    public interface IUpdatePropertyValidator
    {
        void ValidateAndThrow(UpdatePropertyCommand command);
    }
}
