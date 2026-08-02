using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvenTicket
{
    internal class EventController : BaseCRUD<EventModel>
    {
        private int _width, _heigth;

        // atributo que representa um registro de evento
        private EventModel _model;


        private EventView _view;

        // caminho do arquivo de persistência
        private const string FILE_PATH = "eventos.txt";

        // atributo que representa a tabela com todos os eventos
        private List<EventModel> _events;

        // propriedade pública para acesso à lista de eventos (usada pelo RegistrationController)
        public List<EventModel> Events { get { return this._events; } }


        public EventController(int col, int row, Tela tela) : base(col, row, tela)
        {
            this._view = new EventView(
                ConsoleColor.DarkBlue,
                ConsoleColor.White,
                col,
                row
            );

            this._model = new EventModel();

            this._events = new List<EventModel>();

            this.LoadFromFile();

            if (this._events.Count == 0)
            {
                this._events.Add(new EventModel("EVT001", "Tech Summit 2025", "Centro de Convenções",
                    new DateTime(2025, 11, 15), 200));
            }

            this._fields = new List<string>();
            this._fields.Add("Código      : ");
            this._fields.Add("Nome        : ");
            this._fields.Add("Local       : ");
            this._fields.Add("Data        : ");
            this._fields.Add("Capacidade  : ");

            this._width = this._fields[0].Length + 2 + 40;
            this._heigth = this._fields.Count + 2 + 1;
        }


        // retorna o evento correspondente ao código informado, ou null se não encontrado
        public EventModel FindByCode(string code)
        {
            for (int i = 0; i < this._events.Count; i++)
            {
                if (this._events[i].Code == code)
                    return this._events[i];
            }
            return null;
        }


        // retorna quantas inscrições existem para um evento (usado pela regra de capacidade)
        public int CountRegistrations(string eventCode, List<RegistrationModel> registrations)
        {
            int count = 0;
            for (int i = 0; i < registrations.Count; i++)
            {
                if (registrations[i].EventCode == eventCode)
                    count++;
            }
            return count;
        }


        protected override void EnterData(string which)
        {
            if (which == "PK")
            {
                // solicita a entrada da chave primária: código do evento
                int col, row;
                col = this._column + 1 + this._fields[0].Length;
                row = this._row + 2;
                Console.SetCursorPosition(col, row);
                this._model.Code = Console.ReadLine();
            }
            else
            {
                // solicita a entrada dos outros dados do evento
                int col, row;
                col = this._column + 1 + this._fields[0].Length;
                row = this._row + 3;

                this._tela.LimparArea(col, row, this._column + this._width - 2, row + this._heigth - 5);

                Console.SetCursorPosition(col, row);
                this._model.Name = Console.ReadLine();

                row++;
                Console.SetCursorPosition(col, row);
                this._model.Location = Console.ReadLine();

                row++;
                Console.SetCursorPosition(col, row);
                // tratamento de entrada inválida para a data
                DateTime parsedDate;
                string dateInput = Console.ReadLine();
                while (!DateTime.TryParseExact(dateInput, "dd/MM/yyyy",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None, out parsedDate))
                {
                    this._tela.LimparArea(col, row, this._column + this._width - 2, row);
                    Console.SetCursorPosition(col, row);
                    Console.Write("Data inválida. Use dd/MM/yyyy: ");
                    dateInput = Console.ReadLine();
                }
                this._model.Date = parsedDate;

                row++;
                Console.SetCursorPosition(col, row);
                // tratamento de entrada inválida para capacidade
                int parsedCapacity;
                string capInput = Console.ReadLine();
                while (!int.TryParse(capInput, out parsedCapacity) || parsedCapacity <= 0)
                {
                    this._tela.LimparArea(col, row, this._column + this._width - 2, row);
                    Console.SetCursorPosition(col, row);
                    Console.Write("Capacidade inválida. Digite um número inteiro positivo: ");
                    capInput = Console.ReadLine();
                }
                this._model.Capacity = parsedCapacity;
            }
        }


        protected override void ShowData()
        {
            // mostra os dados do evento
            int col, row;
            col = this._column + 1 + this._fields[0].Length;

            row = this._row + 3;
            Console.SetCursorPosition(col, row);
            Console.Write(this._events[this._position].Name);

            row++;
            Console.SetCursorPosition(col, row);
            Console.Write(this._events[this._position].Location);

            row++;
            Console.SetCursorPosition(col, row);
            Console.Write(this._events[this._position].Date.ToString("dd/MM/yyyy"));

            row++;
            Console.SetCursorPosition(col, row);
            Console.Write(this._events[this._position].Capacity);
        }


        private bool FindEvent()
        {
            bool found = false;

            for (int i = 0; i < this._events.Count; i++)
            {
                if (this._events[i].Code == this._model.Code)
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
            int colini = this._column + 1;
            int colfin = this._column + this._width - 1;
            int linha = this._row + this._heigth - 1;

            // preparar a tela de Evento
            this._view.ShowForm();
            this._model.Code = this._view.EnterCode();
            found = this.FindEvent();

            if (found)
            {
                // mostrar os dados do registro encontrado no "banco de dados"
                this._view.ShowData(this._events[this._position]);
                // perguntar se deseja alterar/excluir/voltar
                resp = this._view.Ask("Deseja alterar/excluir/voltar (A/E/V): ");

                // se o usuario desejar alterar
                if (resp == "a")
                {
                    // perguntar os novos dados
                    this._model = this._view.EnterData(this._model.Code);
                    // perguntar se confirma alteração
                    resp = this._view.Ask("Confirma alteração (S/N): ");
                    // se usuário confirmar alteração
                    if (resp == "s")
                    {
                        // atualizar os dados no "banco de dados"
                        this._events[this._position].Name = this._model.Name;
                        this._events[this._position].Location = this._model.Location;
                        this._events[this._position].Date = this._model.Date;
                        this._events[this._position].Capacity = this._model.Capacity;
                    }
                }
                if (resp == "e")
                {
                    // perguntar se confirma exclusão
                    resp = this._view.Ask("Confirma exclusão (S/N): ");
                    // se usuário confirmar exclusão
                    if (resp == "s")
                    {
                        // apagar o registro do "banco de dados"
                        this._events.RemoveAt(this._position);
                    }
                }
            }
            else
            {
                resp = this._view.Ask("Código não encontrado. Deseja cadastrar (S/N): " );

                if (resp == "s")
                {
                    this._model = this._view.EnterData(this._model.Code);
                    resp = this._view.Ask("Confirma cadastro (S/N): ");
                    if (resp == "s")
                    {
                        this._events.Add(
                            new EventModel(this._model.Code, this._model.Name,
                                this._model.Location, this._model.Date, this._model.Capacity)
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
                row, "Cadastro de Eventos");

            row++;
            for (int i = 0; i < this._fields.Count; i++)
            {
                Console.SetCursorPosition(this._column + 1, row);
                Console.Write(this._fields[i]);
                row++;
            }
        }




        // salva todos os eventos em arquivo texto ao encerrar o sistema
        public void SaveToFile()
        {
            using (StreamWriter sw = new StreamWriter(FILE_PATH, false, Encoding.UTF8))
            {
                for (int i = 0; i < this._events.Count; i++)
                {
                    // formato: Código|Nome|Local|Data|Capacidade
                    sw.WriteLine(
                        this._events[i].Code + "|" +
                        this._events[i].Name + "|" +
                        this._events[i].Location + "|" +
                        this._events[i].Date.ToString("dd/MM/yyyy") + "|" +
                        this._events[i].Capacity
                    );
                }
            }
        }



        // carrega os eventos do arquivo texto ao iniciar o sistema
        private void LoadFromFile()
        {
            if (!File.Exists(FILE_PATH)) return;

            using (StreamReader sr = new StreamReader(FILE_PATH, Encoding.UTF8))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] parts = line.Split('|');
                    if (parts.Length == 5)
                    {
                        DateTime date;
                        int capacity;
                        if (DateTime.TryParseExact(parts[3], "dd/MM/yyyy",
                            System.Globalization.CultureInfo.InvariantCulture,
                            System.Globalization.DateTimeStyles.None, out date)
                            && int.TryParse(parts[4], out capacity))
                        {
                            this._events.Add(new EventModel(parts[0], parts[1], parts[2], date, capacity));
                        }
                    }
                }
            }
        }
    }
}
