# 🏆 Quiz de Futebol - ASP.NET Core

Um mini aplicativo web desenvolvido em **ASP.NET Core** que apresenta um quiz de futebol.  
O usuário responde às perguntas e, ao enviar, recebe um feedback mostrando:

- ✅ Quantas perguntas acertou
- 📋 Qual era a resposta correta de cada questão
- ❌ O que marcou nas perguntas que errou

---

## 🚀 Tecnologias Utilizadas
- **.NET 7/8/9** (ASP.NET Core Minimal API)
- **HTML + CSS** simples para interface
- **VS Code** (recomendado) ou qualquer IDE compatível
- **C#** para backend

---

## 🛠️ Como Rodar o Projeto

1. **Instale o .NET SDK**  
   Baixe e instale o .NET 7 ou superior em:  
   [https://dotnet.microsoft.com/download](https://dotnet.microsoft.com/download)

2. **Abra o projeto no VS Code**  
   - File → Open Folder → selecione a pasta `QuizFutebol`
   - Abra o terminal integrado (**Ctrl+`**)

3. **Execute o servidor**
```bash
dotnet run
Acesse no navegador
Abra o endereço exibido no terminal, normalmente:

arduino
Copiar código
http://localhost:5000
Responda o Quiz

Marque as alternativas desejadas

Clique em Enviar

Veja o resultado com acertos e erros

🎯 Personalização
Para mudar o tema ou adicionar novas perguntas:

No arquivo Program.cs → Edite a lista perguntas (enunciado, alternativas e gabarito).

No arquivo index.html → Ajuste as perguntas e alternativas exibidas.
⚠️ Importante: mantenha os name="q1", name="q2"... e os valores value="a|b|c|d coerentes com o gabarito no Program.cs.

📸 Exemplo de Resultado
Após enviar o quiz, o usuário verá algo assim:

less
Copiar código
Você acertou 4 de 6.

Q1. Em qual país foi a final da Copa de 2002?
✅ Acertou! (Sua resposta: Japão)

Q2. Quantos jogadores tem em campo?
❌ Errou. (Você respondeu 10, correto é 11)
...
💡 Melhorias Futuras (Sugestões)
Carregar perguntas de um arquivo JSON

Adicionar pontuação final com barra de progresso

Usar Bootstrap ou TailwindCSS para layout mais moderno

Salvar resultados no banco de dados para estatísticas

👨‍💻 Autor
Projeto criado para trabalho da faculdade.
Desenvolvido em C# com ASP.NET Core, rodando localmente via dotnet run.
