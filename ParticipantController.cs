using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvenTicket
{
    
    internal class ParticipantController : BaseCRUD<ParticipantModel>
    {
        private ParticipantView _view;
        private int _width, _heigth;

        // atributo que representa um registro de participante
        private ParticipantModel _model;

        // atributo que representa a tabela com todos os participantes
        private List<ParticipantModel> _participants;

        // caminho do arquivo de persistência
        private const string FILE_PATH = "participantes.txt";

        // propriedade pública para acesso à lista (usada pelo RegistrationController)
        public List<ParticipantModel> Participants { get { return this._participants; } }


        public ParticipantController(int col, int row, Tela tela) : base(col, row, tela)
        {
            this._view = new ParticipantView( ConsoleColor.DarkBlue,ConsoleColor.White, col, row );
            this._column = col;
            this._row = row;
            this._tela = tela;

            this._model = new ParticipantModel();

            this._participants = new List<ParticipantModel>();

            // carrega os participantes do arquivo de persistência ao iniciar
            this.LoadFromFile();

            // se não houver dados no arquivo, pré-carrega um registro de exemplo
            if (this._participants.Count == 0)
            {
                this._participants.Add(new ParticipantModel("047.123.456-78", "Ana Paula Souza", "ana@email.com"));
            }

            this._fields = new List<string>();
            this._fields.Add("CPF         : ");
            this._fields.Add("Nome        : ");
            this._fields.Add("E-mail      : ");

            this._width = this._fields[0].Length + 2 + 45;
            this._heigth = this._fields.Count + 2 + 1;
        }


        // retorna o participante correspondente ao CPF informado, ou null se não encontrado
        public ParticipantModel FindByCpf(string cpf)
        {
            for (int i = 0; i < this._participants.Count; i++)
            {
                if (this._participants[i].Cpf == cpf)
                    return this._participants[i];
            }
            return null;
        }


        protected override void EnterData(string which)
        {
            if (which == "PK")
            {
                // solicita a entrada da chave primária: CPF do participante
                int col, row;
                col = this._column + 1 + this._fields[0].Length;
                row = this._row + 2;
                Console.SetCursorPosition(col, row);
                this._model.Cpf = Console.ReadLine();
            }
            else
            {
                // solicita a entrada dos outros dados do participante
                int col, row;
                col = this._column + 1 + this._fields[0].Length;
                row = this._row + 3;

                this._tela.LimparArea(col, row, this._column + this._width - 2, row + this._heigth - 5);

                Console.SetCursorPosition(col, row);
                this._model.Name = Console.ReadLine();

                row++;
                Console.SetCursorPosition(col, row);
                this._model.Email = Console.ReadLine();
            }
        }


        protected override void ShowData()
        {
            // mostra os dados do participante
            int col, row;
            col = this._column + 1 + this._fields[0].Length;

            row = this._row + 3;
            Console.SetCursorPosition(col, row);
            Console.Write(this._participants[this._position].Name);

            row++;
            Console.SetCursorPosition(col, row);
            Console.Write(this._participants[this._position].Email);
        }


        private bool FindParticipant()
        {
            bool found = false;

            for (int i = 0; i < this._participants.Count; i++)
            {
                if (this._participants[i].Cpf == this._model.Cpf)
                {
                    found = true;
                    this._position = i;
                    break;
                }
            }

            return found;
        }


        public override void CRUD()
        {
            bool found;
            string resp;

            // preparar a tela de Participante
            this._view.ShowForm();

            // ler CPF pela View
            this._model.Cpf = this._view.EnterCpf();

            // buscar participante
            found = this.FindParticipant();

            if (found)
            {
                // mostrar os dados do participante encontrado
                this._view.ShowData(this._participants[this._position]);

                // perguntar se deseja alterar/excluir/voltar
                resp = this._view.Ask("Deseja alterar/excluir/voltar (A/E/V): ");

                if (resp == "a")
                {
                    // ler novos dados pela View
                    this._model = this._view.EnterData(this._model.Cpf);

                    resp = this._view.Ask("Confirma alteração (S/N): ");

                    if (resp == "s")
                    {
                        this._participants[this._position].Name = this._model.Name;
                        this._participants[this._position].Email = this._model.Email;
                    }
                }

                if (resp == "e")
                {
                    resp = this._view.Ask("Confirma exclusão (S/N): ");

                    if (resp == "s")
                    {
                        this._participants.RemoveAt(this._position);
                    }
                }
            }
            else
            {
                resp = this._view.Ask("CPF não encontrado. Deseja cadastrar (S/N): ");

                if (resp == "s")
                {
                    // CPF já foi digitado no começo, aqui lê só Nome e E-mail
                    this._model = this._view.EnterData(this._model.Cpf);

                    resp = this._view.Ask("Confirma cadastro (S/N): ");

                    if (resp == "s")
                    {
                        this._participants.Add(
                            new ParticipantModel(
                                this._model.Cpf,
                                this._model.Name,
                                this._model.Email
                            )
                        );
                    }
                }
            }
        }


        protected override void ShowForm()
        {
            this._tela.MontarMoldura(this._column, this._row,
                this._column + this._width, this._row + this._heigth);

            int row = this._row + 1;
            this._tela.Centralizar(this._column, this._column + this._width,
                row, "Cadastro de Participantes");

            row++;
            for (int i = 0; i < this._fields.Count; i++)
            {
                Console.SetCursorPosition(this._column + 1, row);
                Console.Write(this._fields[i]);
                row++;
            }
        }


        // salva todos os participantes em arquivo texto ao encerrar o sistema
        public void SaveToFile()
        {
            using (StreamWriter sw = new StreamWriter(FILE_PATH, false, Encoding.UTF8))
            {
                for (int i = 0; i < this._participants.Count; i++)
                {
                    // formato: CPF|Nome|Email
                    sw.WriteLine(
                        this._participants[i].Cpf + "|" +
                        this._participants[i].Name + "|" +
                        this._participants[i].Email
                    );
                }
            }
        }


        // carrega os participantes do arquivo texto ao iniciar o sistema
        private void LoadFromFile()
        {
            if (!File.Exists(FILE_PATH)) return;

            using (StreamReader sr = new StreamReader(FILE_PATH, Encoding.UTF8))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] parts = line.Split('|');
                    if (parts.Length == 3)
                    {
                        this._participants.Add(new ParticipantModel(parts[0], parts[1], parts[2]));
                    }
                }
            }
        }
    }
}
