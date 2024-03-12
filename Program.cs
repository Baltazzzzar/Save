using System.Net.Http.Headers;
using System.Text.Json;

const string CONFIG_FILE = ".config";
string pantry_id = "";

HttpClient httpClient = InitializeHttpClient();

string baseUrl = $"https://getpantry.cloud/apiv1/pantry/{pantry_id}";
string basketUrl = $"{baseUrl}/basket";

HttpResponseMessage response = await httpClient.GetAsync(baseUrl);

if (File.Exists(CONFIG_FILE))
{
    string[] configDetails = File.ReadAllLines(CONFIG_FILE);
    pantry_id = configDetails[0];
}
else
{
    Console.WriteLine("Pantry ID por favor: ");
    pantry_id = Console.ReadLine();
    if (pantry_id.Length > 0)
    {
        File.WriteAllText(CONFIG_FILE, pantry_id);
    }
}

string pantryRawContent = (await response.Content.ReadAsStringAsync()) ?? "";
Pantry? pantry = JsonSerializer.Deserialize<Pantry>(pantryRawContent);

//Console.WriteLine(pantryRawContent);
//Console.WriteLine(pantry.name);


HttpClient InitializeHttpClient()
{
    HttpClient httpClient = new HttpClient();
    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + pantry_id);
    return httpClient;
}

class Pantry
{
    public string? name { get; set; }
    public string? description { get; set; }
    public string? id { get; set; }
    public string? owner { get; set; }
    public string? created_at { get; set; }
    public string? updated_at { get; set; }
}