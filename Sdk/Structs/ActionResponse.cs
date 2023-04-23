namespace SourceSharp.Sdk.Structs;

public struct ActionResponse<T>
{
    public T Response { get; }

    public int Code { get; }

    public ActionResponse(int code, T response)
    {
        Code = code;
        Response = response;
    }
}
