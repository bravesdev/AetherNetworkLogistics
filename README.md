
# Aether Network - Versão 26.5.25

Aether Network é um ecossistema desktop avançado voltado para diagnósticos, monitoramento, triagem e suporte automatizado de infraestruturas de rede e equipamentos de telecomunicações. Desenvolvido em C# com .NET, o software integra ferramentas de análise de tráfego em tempo real, validação segura de acessos corporativos, navegação web integrada com renderização moderna e um assistente de inteligência artificial contextualizado para otimização de fluxos de suporte técnico.

> [!WARNING]
> **Requisito Crítico de Execução:** Este ecossistema foi desenvolvido sobre a plataforma **.NET 10**, garantindo os mais altos padrões de segurança, otimização de memória e integridade nos dispositivos executados.
> Antes de iniciar o programa, é estritamente necessário instalar o ambiente de execução oficial.
> 🔗 **[.NET 10 SDK / Runtime (Site Oficial Microsoft)](https://dotnet.microsoft.com/pt-br/download/dotnet/thank-you/sdk-10.0.300-windows-x64-installer)**
---

## 🚀 Principais Funcionalidades

### 1. Core de Monitoramento e Triagem (`NetworkEngine`)
* **Análise de Links em Tempo Real:** Medição contínua da velocidade de envio/recebimento de dados do link de rede local utilizando estatísticas de adaptadores nativos do Windows (`NetworkInterface`).
* **ICMP Ping Automatizado:** Sistema cíclico controlado por temporizadores assíncronos para acompanhamento de latência e perda de pacotes em múltiplos alvos definidos pelo operador.
* **Histórico Confiável (Screening List):** Mecanismo interno de cadastro, listagem, filtros e persistência de registros em formato JSON (`screening-list.json`), eliminando redundâncias operacionais.
* **Gerenciamento de Atualizações Dinâmicas:** Sincronização remota automatizada por APIs externas para validação de novas versões de compilação disponíveis e patches emergenciais.

### 2. Validação e Controle de Licenciamento (`FrmLogin` / `Program`)
* **Autenticação Baseada em Domínio:** Restrição de acessos nativos a domínios de e-mail corporativos definidos (ex: `@suaempresa.com`).
* **Geração de Token Temporário (OTP):** Envio assíncrono automatizado via SMTP de chaves de validação criptografadas para liberação da estação de trabalho.
* **Persistência de Licença Local:** Emissão de chaves locais de identificação (`ident.lic`) gravadas de forma estruturada no diretório de metadados para evitar logins recorrentes na mesma máquina.

### 3. Assistente de Suporte Técnico IA (`AetherAI` / Sora)
* **LLM Integrada no Core:** Engine de comunicação assíncrona baseada na arquitetura da API do Groq (`llama-3.1-8b-instant`), otimizada para respostas em milissegundos.
* **Modelo Especializado:** Ajustado via instruções de sistema para fornecer diagnósticos em fibra óptica, roteadores, redes estruturadas e hardware técnico corporativo.
* **Protocolo de Segurança WD211:** Mecanismo integrado de proteção comportamental contra linguagem ofensiva ou interações mal-intencionadas.

### 4. Navegador Web Premium Integrado (`FrmNavegador`)
* **Renderização Moderna:** Utiliza o componente `Microsoft.Web.WebView2` baseado no motor Chromium (Edge) de alta performance.
* **Gerenciador Avançado de Abas:** Arquitetura para criação, encerramento e troca dinâmica de abas isoladas em tempo de execução.
* **Design e Fluidização de UI:** Splash screen interna minimalista, barras de progresso vetoriais personalizadas e GDI+ otimizado para evitar problemas de oscilação visual (*flickering*).

### 5. Motor de Estilização Global (`ThemeManager`)
* **Transição Dinâmica de Temas:** Alternância fluida e síncrona entre modos Claro (*Light Mode*) e Escuro (*Dark Mode*).
* **Suporte Completo a Frameworks de UI:** Extensão nativa que intercepta heranças de cores de componentes nativos do WinForms e de elementos visuais avançados do **Guna UI2**.

---

## 🛠️ Tecnologias e Frameworks Utilizados

* **Linguagem:** C# (.NET Core / .NET Framework 10)
* **Interface Gráfica:** Windows Forms integrando componentes gráficos **Guna.UI2.WinForms**.
* **Motor Web:** `Microsoft.Web.WebView2` (Chromium Engine)
* **Comunicação Web e APIs:** `System.Net.Http`, `Newtonsoft.Json` e `System.Text.Json`.
* **Segurança e Sistema Operacional:** Chamadas nativas da API do Windows via Interop Services (`user32.dll` / `kernel32.dll`) para customizações finas de renderização e console.

---

## 📂 Arquitetura do Repositório

```text
├── src/
│   ├── Program.cs                 # Ponto de entrada da aplicação, validação inicial da licença
│   ├── NetworkEngine.cs           # Painel de controle mestre, monitoramento de interface e rotinas centrais
│   ├── NetworkEngine.Designer.cs  # Definições visuais e injeções de componentes gráficos (Guna UI)
│   ├── ThemeManager.cs            # Motor de controle estético e inversão cromática (Dark/Light)
│   ├── FrmLogin.cs                # Portal de acesso com validação via domínio e OTP por e-mail
│   ├── FrmNavegador.cs            # Web Browser multi-abas otimizado baseado em WebView2
│   ├── AetherAI.cs                # Camada de comunicação de rede com o provedor de LLM (Groq API)
│   ├── AetherAI_Form.cs           # Interface conversacional com a assistente técnica Sora
│   ├── FrmAetherMsg.cs            # Caixa de diálogo (MessageBox) customizada e estilizada
│   └── FrmInfomations.cs          # Visualizador de metadados, termos e créditos do ecossistema
└── metadados/                     # Arquivos locais gerados em tempo de execução (JSON/LIC)

```

---

## ⚙️ Pré-requisitos para Desenvolvimento

Para compilar e modificar o Aether Network localmente, certifique-se de preencher os seguintes requisitos em sua máquina:

1. **IDE:** Visual Studio 2022 ou mais recente.
2. **Workload:** "Desenvolvimento para desktop com o .NET 10".
3. **Dependências Nuget Requeridas:**
* `Guna.UI2.WinForms`
* `Microsoft.Web.WebView2`
* `Newtonsoft.Json`


4. **Runtime do Usuário Final:** Ter o *WebView2 Runtime* instalado no sistema operacional corporativo.

---

## 📦 Configuração e Instalação

1. Clone o repositório em sua estação de desenvolvimento:
```bash
git clone https://github.com/zwendersonbr/aethernetworklogistics.git

```


2. Abra o arquivo de solução (`.sln`) no Visual Studio.
3. Restaure os pacotes NuGet do projeto.
4. No arquivo `AetherAI.cs`, configure a sua chave privada da API em `API_KEY` (se necessário alterar o modelo de inferência).
5. Altere as propriedades do servidor de saída SMTP no arquivo `FrmLogin.cs` para direcionar as mensagens de liberação de login à sua infraestrutura de e-mail própria.
6. Compile o projeto em modo `Release` ou `Debug`.

---

## 🛡️ Políticas de Segurança Operacional

O sistema possui uma camada nativa para integridade da aplicação e proteção da equipe de suporte técnico:

* **Validação por Domínio:** O ecossistema bloqueia tentativas de registro se o e-mail informado não pertencer explicitamente às regras corporativas configuradas.
* O sistema trabalha de forma 100% ofiline após o primeiro acesso. Sendo assim não há necessidade de estar conectado a internet para diagnostico dos equipamentos.

---

## 📝 Licença e Propriedade

Este projeto é um software de código aberto, licenciado sob os termos da Licença MIT. Você é livre para modificar, distribuir e utilizar o código para fins privados ou comerciais, desde que inclua os devidos créditos e o aviso de licença original em todas as cópias ou partes substanciais do software.
