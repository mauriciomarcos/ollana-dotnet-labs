
using OllamaSharp;

Console.WriteLine("--- Iniciando Chat com Ollama ---");

var urlOllamaServer = new Uri("http://localhost:11434");
var ollamaClient = new OllamaApiClient(urlOllamaServer);

// Listar os modelos locais e exibir
var models = await ollamaClient.ListLocalModelsAsync();
if (models.Any())
{
    foreach (var model in models)
        Console.WriteLine(model.Name);
}
else
{
    Console.WriteLine("Nenhum modelo local encontrado. Baixando modelo phi3:latest");

    var retunInstallModel = ollamaClient.PullModelAsync("phi3:latest");
    await foreach (var item in retunInstallModel)
    {
        Console.WriteLine($"Status da instalação do modelo: {Math.Round((decimal)item?.Percent!)}%");
    }

    Console.WriteLine("Modelo phi3:latest instalado com sucesso");
}

// Selecionando o modelo a ser utilzado
ollamaClient.SelectedModel = "phi3:latest";

// Interagindo com o modelo
Console.WriteLine("Ask your question...");

var ollamaChat = new Chat(ollamaClient);
var userPrompt = Console.ReadLine() ?? string.Empty;

await foreach (var answer in ollamaChat.SendAsync(userPrompt))
{
    Console.Write(answer);
}

Console.WriteLine("--- Fim ---");