namespace Million.Technical.Test.Application.Commands.Validations
{
    public interface ICreatePropertyTraceValidator
    {
        void ValidateAndThrow(CreatePropertyTraceCommand command);
    }
}