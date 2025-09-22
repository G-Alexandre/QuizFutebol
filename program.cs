using System.Text.Encodings.Web;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRouting();
var app = builder.Build();

app.UseDefaultFiles();   // procura index.html
app.UseStaticFiles();    // serve wwwroot

// Banco "hardcoded" de perguntas (poderia vir de JSON/BD)
var perguntas = new List<Pergunta>
{
    new("Em qual país foi a final da Copa do Mundo de 2002?", new[]{
        ("a","Japão"), ("b","Coreia do Sul"), ("c","Alemanha"), ("d","Brasil")
    }, respostaCorreta:"a"),
    new("Quantos jogadores cada equipe tem em campo no futebol?", new[]{
        ("a","10"), ("b","11"), ("c","12"), ("d","9")
    }, "b"),
    new("Quem é o maior artilheiro da história das Copas do Mundo?", new[]{
        ("a","Pelé"), ("b","Miroslav Klose"), ("c","Ronaldo Fenômeno"), ("d","Lionel Messi")
    }, "b"),
    new("Quantos títulos de Copa do Mundo a Seleção Brasileira possui?", new[]{
        ("a","4"), ("b","5"), ("c","6"), ("d","3")
    }, "b"),
    new("Qual é o tempo regulamentar de uma partida (descontos à parte)?", new[]{
        ("a","80 minutos"), ("b","100 minutos"), ("c","90 minutos"), ("d","70 minutos")
    }, "c"),
    new("Qual destes é um cartão disciplinar no futebol?", new[]{
        ("a","Cartão azul"), ("b","Cartão verde"), ("c","Cartão roxo"), ("d","Cartão vermelho")
    }, "d"),
};

// Endpoint para correção
app.MapPost("/submit", async (HttpContext ctx) =>
{
    // Lê o formulário (name="q1"...)
    var form = await ctx.Request.ReadFormAsync();

    var resultados = new List<ResultadoPergunta>();
    int acertos = 0;

    for (int i = 0; i < perguntas.Count; i++)
    {
        var p = perguntas[i];
        var nomeCampo = $"q{i+1}";
        var respostaUsuario = form[nomeCampo].ToString(); // pode vir vazio

        bool correto = respostaUsuario == p.RespostaCorreta;
        if (correto) acertos++;

        // Descobre textos da alternativa marcada e da correta
        string textoMarcada = p.Alternativas.FirstOrDefault(a => a.value == respostaUsuario).text ?? "(não marcada)";
        string textoCorreta = p.Alternativas.First(a => a.value == p.RespostaCorreta).text;

        resultados.Add(new ResultadoPergunta(
            Numero: i+1,
            Enunciado: p.Enunciado,
            Marcada: textoMarcada,
            Correta: textoCorreta,
            Acertou: correto
        ));
    }

    // Renderiza HTML simples com feedback
    string html = HtmlRenderer.RenderResultado(perguntas.Count, acertos, resultados);
    ctx.Response.ContentType = "text/html; charset=utf-8";
    await ctx.Response.WriteAsync(html);
});

app.Run();

record Pergunta(string Enunciado, (string value, string text)[] Alternativas, string RespostaCorreta);
record ResultadoPergunta(int Numero, string Enunciado, string Marcada, string Correta, bool Acertou);

static class HtmlRenderer
{
    public static string RenderResultado(int total, int acertos, List<ResultadoPergunta> itens)
    {
        var safe = HtmlEncoder.Default;
        var linhas = string.Join("", itens.Select(r =>
$@"<div class=""card { (r.Acertou ? "ok" : "no") }"">
  <div class=""q""><b>Q{r.Numero}.</b> {safe.Encode(r.Enunciado)}</div>
  <div class=""a"">{(r.Acertou ? "✅ Acertou!" : "❌ Errou.")}</div>
  <div class=""det"">
    Sua resposta: <b>{safe.Encode(r.Marcada)}</b><br>
    Correta: <b>{safe.Encode(r.Correta)}</b>
  </div>
</div>"));

        return $@"<!doctype html>
<html lang=""pt-br"">
<head>
<meta charset=""utf-8"">
<meta name=""viewport"" content=""width=device-width, initial-scale=1"">
<title>Resultado do Quiz</title>
<style>
  body{{font-family:system-ui,-apple-system,Segoe UI,Roboto,Ubuntu;max-width:900px;margin:40px auto;padding:0 16px;}}
  .score{{font-size:22px;margin-bottom:12px}}
  .again{{display:inline-block;margin:8px 0 24px;text-decoration:none;border:1px solid #ccc;padding:10px 14px;border-radius:10px}}
  .card{{border:1px solid #e5e7eb;border-radius:12px;padding:14px 16px;margin-bottom:12px}}
  .card.ok{{border-color:#86efac;background:#f0fdf4}}
  .card.no{{border-color:#fca5a5;background:#fef2f2}}
  .q{{margin-bottom:8px}}
  .det{{color:#374151}}
</style>
</head>
<body>
  <h1>Resultado do Quiz</h1>
  <div class=""score"">Você acertou <b>{acertos}</b> de <b>{total}</b>.</div>
  <a class=""again"" href=""/"">Refazer quiz</a>
  {linhas}
</body>
</html>";
    }
}
