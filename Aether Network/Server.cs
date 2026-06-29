using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace aether
{
    public class NetworkConfig
    {
        public string ServerIp { get; set; }
        public int ServerPort { get; set; }
    }

    public class ServerPreferences
    {
        public NetworkConfig NetworkConfig { get; set; }
    }

    public class Server
    {
        // IP e Porta foram declarados sem valores pré-definidos (hardcoded)
        private string _ipServidor;
        private int _portaServidor;

        private Socket _socket;
        private string _nomeTecnico;
        private bool _pararReconexao = false;

        public event Action<string> OnMensagemRecebida;
        public event Action<List<string>> OnListaTecnicosRecebida;

        public bool IsConnected => _socket != null && _socket.Connected;

        public Server(string nomeTecnico)
        {
            _nomeTecnico = nomeTecnico;
            CarregarPreferencias();
        }

        private void CarregarPreferencias()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "metadados", "preferences.json");

            try
            {
                if (File.Exists(path))
                {
                    string jsonString = File.ReadAllText(path);
                    var preferences = JsonSerializer.Deserialize<ServerPreferences>(jsonString);

                    if (preferences?.NetworkConfig != null)
                    {
                        // Armazena estritamente o que estiver no arquivo de configuração
                        _ipServidor = preferences.NetworkConfig.ServerIp;
                        _portaServidor = preferences.NetworkConfig.ServerPort;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar configurações de rede: {ex.Message}");
            }
        }

        // Conecta consumindo unicamente as variáveis alimentadas pelo arquivo
        public bool Conectar()
        {
            // Valida se o arquivo forneceu dados válidos antes de iniciar a thread de conexão
            if (string.IsNullOrEmpty(_ipServidor) || _portaServidor <= 0)
            {
                Console.WriteLine("Falha ao conectar: Nenhum IP ou Porta configurado no arquivo preferences.json.");
                return false;
            }

            _pararReconexao = false;

            // Inicia o loop de conexão em segundo plano apontando apenas para os dados do arquivo
            Task.Run(() => LoopConexao(_ipServidor, _portaServidor));

            return true;
        }

        private async Task LoopConexao(string ip, int porta)
        {
            while (!_pararReconexao)
            {
                if (!IsConnected)
                {
                    try
                    {
                        FecharSocket();

                        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        _socket.Connect(ip, porta);

                        byte[] data = Encoding.UTF8.GetBytes(_nomeTecnico);
                        _socket.Send(data);

                        EscutarServidor();
                    }
                    catch (Exception)
                    {
                        // Falha ou queda de conexão capturada. Avança para o Delay de tentativa.
                    }
                }

                if (!_pararReconexao && !IsConnected)
                {
                    await Task.Delay(5000);
                }
            }
        }

        private void EscutarServidor()
        {
            byte[] buffer = new byte[2048];

            while (_socket != null && _socket.Connected)
            {
                try
                {
                    int received = _socket.Receive(buffer);

                    if (received <= 0)
                    {
                        break;
                    }

                    string msg = Encoding.UTF8.GetString(buffer, 0, received);

                    if (msg.StartsWith("USERS|"))
                    {
                        var lista = msg.Replace("USERS|", "").Split(',').ToList();
                        OnListaTecnicosRecebida?.Invoke(lista);
                    }
                    else
                    {
                        OnMensagemRecebida?.Invoke(msg);
                    }
                }
                catch
                {
                    break;
                }
            }
        }

        public void SolicitarListaTecnicos()
        {
            if (IsConnected)
            {
                byte[] data = Encoding.UTF8.GetBytes("GET_USERS");
                _socket.Send(data);
            }
        }

        public void EnviarMensagem(string destino, string id, string categoria, string diagnostico, string nota)
        {
            if (!IsConnected) return;

            try
            {
                string payload = $"{destino}|{id}|{categoria}|{diagnostico}|{nota}";
                byte[] data = Encoding.UTF8.GetBytes(payload);
                _socket.Send(data);
            }
            catch (Exception) { }
        }

        private void FecharSocket()
        {
            if (_socket != null)
            {
                try { _socket.Shutdown(SocketShutdown.Both); } catch { }
                try { _socket.Close(); } catch { }
                _socket = null;
            }
        }

        public void Desconectar()
        {
            _pararReconexao = true;
            FecharSocket();
        }
    }
}