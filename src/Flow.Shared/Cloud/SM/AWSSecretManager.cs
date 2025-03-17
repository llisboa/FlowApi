using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;

namespace Flow.Shared.Cloud.SM;
public class AWSSecretManager
{
    private readonly IAmazonSecretsManager _amazonSecretsManager;

    public AWSSecretManager(IAmazonSecretsManager amazonSecretsManager)
    {
        _amazonSecretsManager = amazonSecretsManager;
    }

    public async Task<string> GetSecretAsync(string secretId, string versionStage)
    {
        GetSecretValueRequest request = new()
        {
            SecretId = secretId,
            VersionStage = versionStage
        };
        var response = await _amazonSecretsManager.GetSecretValueAsync(request);
        if (response == null)
            return "";
        return response.SecretString;
    }
}
