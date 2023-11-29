namespace ManagementBook.Infra.Cross.Errors;
public class InternalError : BaseError
{
    public InternalError(string message) : base(errorCode: ECodeError.Unhandled, message)
    {
        
    }
}
