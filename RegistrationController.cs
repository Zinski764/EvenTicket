using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvenTicket
{
    internal class RegistrationController : BaseCRUD<RegistrationModel>
    {

        private int _width, _heigth;

        // atributo que representa um registro de inscrição
        private RegistrationModel _model;

        // atributo que representa a tabela com todas as inscrições
        private List<RegistrationModel> _registrations;

        // referências aos controllers de evento e participante para validação das inscrições

        private EventController _eventController;

        private ParticipantController _participantController;

        private RegistrationView _view;

        // caminho do arquivo de persistência
        private const string FILE_PATH = "inscricoes.txt";

        // constantes de layout dos relatórios
        // tela: colunas 0-79 (80 cols), linhas 0-24 (25 linhas)
        private const int RLT_COL         = 1;
        private const int RLT_ROW         = 4;
        private const int RLT_CF          = 78;
        private const int RLT_LF          = 22;
        private const int RLT_ROW_TITLE   = RLT_ROW + 1;
        private const int RLT_ROW_HEADER  = RLT_ROW + 2;
        private const int RLT_ROW_DATA    = RLT_ROW + 3;
        private const int RLT_ROW_LAST    = RLT_LF  - 1;
        private const int RLT_ROW_PROMPT  = RLT_LF  + 1;
        // colunas dos campos dos relatórios
        private const int RLT_COL_CODE    = 3;
        private const int RLT_COL_EVENT   = 14;
        private const int RLT_COL_CPF     = 38;
        private const int RLT_COL_NAME    = 52;
        private const int RLT_COL_DATE    = 68;


        public RegistrationController(int col, int row, Tela tela,
            EventController eventController, ParticipantController participantController) : base(col, row, tela)
        {
            this._view = new RegistrationView(ConsoleColor.DarkBlue,ConsoleColor.White,col,row);
            this._column = col;
            this._row = row;
            this._tela = tela;

            this._model = new RegistrationModel();

            this._registrations = new List<RegistrationModel>();

            this._eventController = eventController;
            this._participantController = participantController;

            // carrega as inscrições do arquivo de persistência ao iniciar
            this.LoadFromFile();

            this._fields = new List<string>();
            this._fields.Add("Cód. Evento   : ");
            this._fields.Add("Nome Evento   : ");
            this._fields.Add("CPF Part.     : ");
            this._fields.Add("Nome Part.    : ");
            this._fields.Add("Dt. Inscrição : ");

            this._width = this._fields[0].Length + 2 + 40;
            this._heigth = this._fields.Count + 2 + 1;
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
                this._model.EventCode = Console.ReadLine();

                // exibe o nome do evento logo abaixo do código informado
                EventModel ev = this._eventController.FindByCode(this._model.EventCode);
                row++;
                Console.SetCursorPosition(col, row);
                if (ev != null)
                    Console.Write(ev.Name);
                else
                    Console.Write("(evento não encontrado)");
            }
            else
            {
                // solicita a entrada do CPF do participante
                int col, row;
                col = this._column + 1 + this._fields[0].Length;
                row = this._row + 4;

                this._tela.LimparArea(col, row, this._column + this._width - 2, row + this._heigth - 5);

                Console.SetCursorPosition(col, row);
                this._model.ParticipantCpf = Console.ReadLine();

                // exibe o nome do participante logo abaixo do CPF informado
                ParticipantModel participant = this._participantController.FindByCpf(this._model.ParticipantCpf);
                row++;
                Console.SetCursorPosition(col, row);
                if (participant != null)
                    Console.Write(participant.Name);
                else
                    Console.Write("(participante não encontrado)");

                // data de inscrição é preenchida automaticamente com a data atual
                this._model.RegistrationDate = DateTime.Today;
            }
        }


        protected override void ShowData()
        {
            // mostra os dados da inscrição
            int col, row;
            col = this._column + 1 + this._fields[0].Length;

            // nome do evento
            row = this._row + 3;
            EventModel ev = this._eventController.FindByCode(this._registrations[this._position].EventCode);
            Console.SetCursorPosition(col, row);
            Console.Write(ev != null ? ev.Name : "");

            // CPF do participante
            row++;
            Console.SetCursorPosition(col, row);
            Console.Write(this._registrations[this._position].ParticipantCpf);

            // nome do participante
            row++;
            ParticipantModel participant = this._participantController.FindByCpf(
                this._registrations[this._position].ParticipantCpf);
            Console.SetCursorPosition(col, row);
            Console.Write(participant != null ? participant.Name : "");

            // data de inscrição
            row++;
            Console.SetCursorPosition(col, row);
            Console.Write(this._registrations[this._position].RegistrationDate.ToString("dd/MM/yyyy"));
        }


        private bool FindRegistration()
        {
            bool found = false;

            for (int i = 0; i < this._registrations.Count; i++)
            {
                if (this._registrations[i].EventCode == this._model.EventCode &&
                    this._registrations[i].ParticipantCpf == this._model.ParticipantCpf)
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

            // preparar a tela de Inscrição
            this._view.ShowForm();
            this.EnterData("PK");
            this.EnterData("DT");
            found = this.FindRegistration();

            if (found)
            {
                // mostrar os dados da inscrição encontrada
                EventModel ev = this._eventController.FindByCode(this._registrations[this._position].EventCode);

                ParticipantModel participant =
                    this._participantController.FindByCpf(
                        this._registrations[this._position].ParticipantCpf);

                this._view.ShowData(
                    this._registrations[this._position],
                    ev,
                    participant
                );
                // perguntar se deseja cancelar/voltar
                resp = this._view.Ask("Deseja cancelar inscrição/voltar (C/V): ");

                if (resp == "c")
                {
                    // perguntar se confirma cancelamento
                    resp = this._view.Ask("Confirma cancelamento (S/N): ");
                    // se usuário confirmar cancelamento, a vaga é liberada automaticamente
                    if (resp == "s")
                    {
                        // cancelar a inscrição libera a vaga automaticamente (regra de negócio RN03)
                        this._registrations.RemoveAt(this._position);
                    }
                }
            }
            else
            {
                // verificar se o evento existe antes de oferecer inscrição (regra de negócio RN01)
                EventModel ev = this._eventController.FindByCode(this._model.EventCode);
                if (ev == null)
                {
                    this._view.Ask("Evento não encontrado no cadastro. Tecle Enter: ");
                    return;
                }

                // verificar se há vagas disponíveis (regra de negócio RN02)
                int ocupadas = this._registrations.FindAll(r => r.EventCode == this._model.EventCode).Count;
                if (ocupadas >= ev.Capacity)
                {
                    this._view.Ask("Evento sem vagas disponíveis. Tecle Enter: ");
                    return;
                }

                resp = this._view.Ask("Evento sem inscrição. Deseja inscrever participante (S/N): ");

                if (resp == "s")
                {
       
                    // verificar se o participante existe (regra de negócio RN04)
                    ParticipantModel participant = this._participantController.FindByCpf(this._model.ParticipantCpf);
                    if (participant == null)
                    {
                        this._view.Ask("CPF não encontrado no cadastro de participantes. Tecle Enter: ");
                        return;
                    }

                    // verificar se o participante já está inscrito neste evento (regra de negócio RN05)
                    bool alreadyRegistered = false;
                    for (int i = 0; i < this._registrations.Count; i++)
                    {
                        if (this._registrations[i].EventCode == this._model.EventCode &&
                            this._registrations[i].ParticipantCpf == this._model.ParticipantCpf)
                        {
                            alreadyRegistered = true;
                            break;
                        }
                    }
                    if (alreadyRegistered)
                    {
                        this._view.Ask("Participante já inscrito neste evento. Tecle Enter: ");
                        return;
                    }

                    // exibir data de inscrição preenchida automaticamente
                    this.ShowRegistrationDate();

                    resp = this._view.Ask("Confirma inscrição (S/N): ");
                    if (resp == "s")
                    {
                        this._registrations.Add(
                            new RegistrationModel(this._model.EventCode,
                                this._model.ParticipantCpf,
                                this._model.RegistrationDate)
                        );
                    }
                }
            }
        }


        // relatório de eventos com vagas disponíveis
        public void ReportAvailableEvents()
        {
            this._view.ShowAvailableEventsReportForm();

            int row = RLT_ROW_DATA;
            int count = 0;

            for (int i = 0; i < this._eventController.Events.Count; i++)
            {
                EventModel ev = this._eventController.Events[i];
                int ocupadas = this._registrations.FindAll(r => r.EventCode == ev.Code).Count;
                int vagas = ev.Capacity - ocupadas;

                if (vagas > 0)
                {
                    if (row > RLT_ROW_LAST) break;
                    this._view.ShowAvailableEventRow(row, ev, vagas);
                    row++;
                    count++;
                }
            }

            if (count == 0)
            {
                this._view.ShowCenteredMessage(row, "Nenhum evento com vagas disponíveis.");
            }

            this._view.WaitReport();
        }



        // relatório de participantes inscritos em um evento específico
        public void ReportParticipantsByEvent()
        {
            string code = this._view.AskEventCodeForReport();

            EventModel ev = this._eventController.FindByCode(code);
            if (ev == null)
            {
                this._tela.Centralizar(RLT_COL, RLT_CF, RLT_ROW_DATA, "Evento não encontrado.");
                this._view.Ask("Tecle Enter para voltar: ");
                return;
            }

            this._view.ShowParticipantsByEventReportForm(ev);

            int row = RLT_ROW_DATA;
            int count = 0;

            for (int i = 0; i < this._registrations.Count; i++)
            {
                if (this._registrations[i].EventCode == code)
                {
                    if (row > RLT_ROW_LAST) break;

                    ParticipantModel participant = this._participantController.FindByCpf(
                        this._registrations[i].ParticipantCpf);

                    this._view.ShowParticipantRow( row,this._registrations[i],participant );

                    row++;
                    count++;
                }
            }

            if (count == 0)
            {
                this._view.ShowCenteredMessage(row, "Nenhum participante inscrito neste evento.");
            }

            this._view.Ask("Tecle Enter para voltar: ");
        }


        // monta a moldura e o cabeçalho comuns ao relatório de eventos disponíveis
        private void ShowReportForm(string titulo)
        {
            this._tela.MontarMoldura(RLT_COL, RLT_ROW, RLT_CF, RLT_LF);
            this._tela.Centralizar(RLT_COL, RLT_CF, RLT_ROW_TITLE, titulo);

            Console.SetCursorPosition(RLT_COL_CODE,  RLT_ROW_HEADER); Console.Write("Código");
            Console.SetCursorPosition(RLT_COL_EVENT, RLT_ROW_HEADER); Console.Write("Nome do Evento");
            Console.SetCursorPosition(RLT_COL_CPF,   RLT_ROW_HEADER); Console.Write("Local");
            Console.SetCursorPosition(RLT_COL_NAME,  RLT_ROW_HEADER); Console.Write("Data");
            Console.SetCursorPosition(RLT_COL_DATE,  RLT_ROW_HEADER); Console.Write("Vagas");
        }


        // imprime uma linha do relatório de eventos com vagas disponíveis
        private void ShowAvailableEventRow(int row, EventModel ev, int vagas)
        {
            string name     = this.Truncate(ev.Name,     RLT_COL_CPF  - RLT_COL_EVENT - 1);
            string location = this.Truncate(ev.Location, RLT_COL_NAME - RLT_COL_CPF   - 1);

            Console.SetCursorPosition(RLT_COL_CODE,  row); Console.Write(this.Truncate(ev.Code, RLT_COL_EVENT - RLT_COL_CODE - 1));
            Console.SetCursorPosition(RLT_COL_EVENT, row); Console.Write(name);
            Console.SetCursorPosition(RLT_COL_CPF,   row); Console.Write(location);
            Console.SetCursorPosition(RLT_COL_NAME,  row); Console.Write(ev.Date.ToString("dd/MM/yyyy"));
            Console.SetCursorPosition(RLT_COL_DATE,  row); Console.Write(vagas);
        }


        // trunca uma string para não ultrapassar o limite de colunas disponível
        private string Truncate(string text, int maxLength)
        {
            if (text.Length <= maxLength)
                return text;
            return text.Substring(0, maxLength - 1) + "…";
        }


        // exibe a data de inscrição preenchida automaticamente
        private void ShowRegistrationDate()
        {
            int col = this._column + 1 + this._fields[0].Length;
            int row = this._row + 6;
            Console.SetCursorPosition(col, row);
            Console.Write(this._model.RegistrationDate.ToString("dd/MM/yyyy"));
        }


        protected override void ShowForm()
        {
            this._tela.MontarMoldura(this._column, this._row,
                this._column + this._width, this._row + this._heigth);

            int row = this._row + 1;
            this._tela.Centralizar(this._column, this._column + this._width,
                row, "Cadastro de Inscrições");

            row++;
            for (int i = 0; i < this._fields.Count; i++)
            {
                Console.SetCursorPosition(this._column + 1, row);
                Console.Write(this._fields[i]);
                row++;
            }
        }


        // salva todas as inscrições em arquivo texto ao encerrar o sistema
        public void SaveToFile()
        {
            using (StreamWriter sw = new StreamWriter(FILE_PATH, false, Encoding.UTF8))
            {
                for (int i = 0; i < this._registrations.Count; i++)
                {
                    // formato: CódEvento|CPFParticipante|DataInscrição
                    sw.WriteLine(
                        this._registrations[i].EventCode + "|" +
                        this._registrations[i].ParticipantCpf + "|" +
                        this._registrations[i].RegistrationDate.ToString("dd/MM/yyyy")
                    );
                }
            }
        }


        // carrega as inscrições do arquivo texto ao iniciar o sistema
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
                        DateTime date;
                        if (DateTime.TryParseExact(parts[2], "dd/MM/yyyy",
                            System.Globalization.CultureInfo.InvariantCulture,
                            System.Globalization.DateTimeStyles.None, out date))
                        {
                            this._registrations.Add(new RegistrationModel(parts[0], parts[1], date));
                        }
                    }
                }
            }
        }
    }
}
