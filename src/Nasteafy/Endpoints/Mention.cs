namespace Nasteafy.Endpoints
{
    public class Mention
    {
        // It is not recommended to use Command directly as Body parameter, it is better to create request DTOs.
        // In all the endpoints that you have command as a Body parameter, create a DTO instead and map it onto the command in the Endpoint.
        // By using command directly you are coupling presentation level with application level, when they should not be coupled by clean architecture, you don't want to leak application details into the API
        // Sometimes parameters are called differently or command has more params than what is expected to come from API, so you:
        // 1. Create dtos and fill them with properties required only for API to proceed, naming properties accordingly 
        // 2. Create command that can have same properties, sometimes with different names, according to application layer, sometimes additional properties.
        // 3. Map it manually or via extensions if dto's and commands are big enough. 
    }
}
