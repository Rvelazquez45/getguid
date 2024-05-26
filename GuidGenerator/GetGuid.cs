using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Net;

namespace GuidGenerator;

public class GetGuid
{
    private readonly ILogger<GetGuid> _logger;

    public GetGuid(ILogger<GetGuid> logger)
    {
        _logger = logger;
    }

    //http://localhost:7272/api/GetGuid?count=10
    [Function("GetGuid")]
    public IActionResult Run
    (
        [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req
    )
    {
        _logger.LogInformation(message: "Started the GetGuid function Call");

        string numberOfGuidsText = req.Query["count"];
        int numberOfGuids = 1;
        List<string> guids = new();

        if // If both are true, we have an actual value
        (
            numberOfGuidsText is not null &&
            int.TryParse(numberOfGuidsText, out numberOfGuids)// this would set numberOfGuids to 0 if tryparse returns false
        )
        {
            _logger.LogInformation(message: $"Number of GUIDs requested: {numberOfGuids}");
        }
        else
        {
            _logger.LogInformation(message: "Unknown number of GUIDs requested, defaulting to 1.");

            numberOfGuids = 1;
        }

        for (int i = 0; i < numberOfGuids; i++)
        { guids.Add(Guid.NewGuid().ToString()); }

        // This will automatically serialize the guids object to JSON 
        // and set the appropriate content type in the HTTP response.
        OkObjectResult okObjectResult = new (value: guids);

        return okObjectResult;
    }
}
