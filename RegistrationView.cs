using System;
using System.Collections.Generic;

namespace EvenTicket
{
    // Classe responsável pela interface do módulo de Inscrições.
    // Também é utilizada para apresentação dos relatórios do sistema.
    internal class RegistrationView : Tela
    {
        private int _column, _row, _width, _heigth;
        private List<string> _fields;

        private const int RLT_COL = 1;
        private const int RLT_ROW = 4;
        private const int RLT_CF = 78;
        private const int RLT_LF = 22;
        private const int RLT_ROW_TITLE = RLT_ROW + 1;
        private const int RLT_ROW_HEADER = RLT_ROW + 2;
        private const int RLT_ROW_DATA = RLT_ROW + 3;
        private const int RLT_ROW_LAST = RLT_LF - 1;
        private const int RLT_ROW_PROMPT = RLT_LF + 1;

        private const int RLT_COL_CODE = 3;
        private const int RLT_COL_EVENT = 14;
        private const int RLT_COL_CPF = 38;
        private const int RLT_COL_NAME = 52;
        private const int RLT_COL_DATE = 68;

        public int ReportDataRow { get { return RLT_ROW_DATA; } }
        public int ReportLastRow { get { return RLT_ROW_LAST; } }

        public RegistrationView(ConsoleColor cf, ConsoleColor ct, int col, int row)
            : base(cf, ct)
        {
            this._column = col;
            this._row = row;

            this._fields = new List<string>();
            this._fields.Add("Cód. Evento   : ");
            this._fields.Add("Nome Evento   : ");
            this._fields.Add("CPF Part.     : ");
            this._fields.Add("Nome Part.    : ");
            this._fields.Add("Dt. Inscrição : ");

            this._width = this._fields[0].Length + 2 + 40;
            this._heigth = this._fields.Count + 2 + 1;
        }

        // Exibe o formulário utilizado para cadastro e consulta de inscrições.
        public void ShowForm()
        {
            this.MontarMoldura(this._column, this._row,
                this._column + this._width, this._row + this._heigth);

            int row = this._row + 1;
            this.Centralizar(this._column, this._column + this._width,
                row, "Cadastro de Inscrições");

            row++;
            for (int i = 0; i < this._fields.Count; i++)
            {
                Console.SetCursorPosition(this._column + 1, row);
                Console.Write(this._fields[i]);
                row++;
            }
        }

        public string EnterEventCode(EventModel ev)
        {
            int col = this._column + 1 + this._fields[0].Length;
            int row = this._row + 2;

            Console.SetCursorPosition(col, row);
            string code = Console.ReadLine() ?? "";

            row++;
            Console.SetCursorPosition(col, row);

            if (ev != null)
                Console.Write(ev.Name);
            else
                Console.Write("(evento não encontrado)");

            return code;
        }

        public string EnterParticipantCpf(ParticipantModel participant)
        {
            int col = this._column + 1 + this._fields[0].Length;
            int row = this._row + 4;

            this.LimparArea(col, row,
                this._column + this._width - 2,
                row + this._heigth - 5);

            Console.SetCursorPosition(col, row);
            string cpf = Console.ReadLine() ?? "";

            row++;
            Console.SetCursorPosition(col, row);

            if (participant != null)
                Console.Write(participant.Name);
            else
                Console.Write("(participante não encontrado)");

            return cpf;
        }

        // Exibe os dados de uma inscrição localizada.
        public void ShowData(RegistrationModel reg, EventModel ev, ParticipantModel participant)
        {
            int col = this._column + 1 + this._fields[0].Length;

            int row = this._row + 3;
            Console.SetCursorPosition(col, row);
            Console.Write(ev != null ? ev.Name : "");

            row++;
            Console.SetCursorPosition(col, row);
            Console.Write(reg.ParticipantCpf);

            row++;
            Console.SetCursorPosition(col, row);
            Console.Write(participant != null ? participant.Name : "");

            row++;
            Console.SetCursorPosition(col, row);
            Console.Write(reg.RegistrationDate.ToString("dd/MM/yyyy"));
        }

        public void ShowRegistrationDate(DateTime date)
        {
            int col = this._column + 1 + this._fields[0].Length;
            int row = this._row + 6;

            Console.SetCursorPosition(col, row);
            Console.Write(date.ToString("dd/MM/yyyy"));
        }

        // Exibe uma pergunta ao usuário e retorna sua resposta.
        public string Ask(string question)
        {
            int colini = this._column + 1;
            int colfin = this._column + this._width - 1;
            int linha = this._row + this._heigth - 1;

            return this.Perguntar(question, linha, colini, colfin);
        }

        // Monta o cabeçalho do relatório de eventos com vagas disponíveis.
        public void ShowAvailableEventsReportForm()
        {
            this.MontarMoldura(RLT_COL, RLT_ROW, RLT_CF, RLT_LF);
            this.Centralizar(RLT_COL, RLT_CF, RLT_ROW_TITLE,
                "Eventos com Vagas Disponíveis");

            Console.SetCursorPosition(RLT_COL_CODE, RLT_ROW_HEADER);
            Console.Write("Código");
            Console.SetCursorPosition(RLT_COL_EVENT, RLT_ROW_HEADER);
            Console.Write("Nome do Evento");
            Console.SetCursorPosition(RLT_COL_CPF, RLT_ROW_HEADER);
            Console.Write("Local");
            Console.SetCursorPosition(RLT_COL_NAME, RLT_ROW_HEADER);
            Console.Write("Data");
            Console.SetCursorPosition(RLT_COL_DATE, RLT_ROW_HEADER);
            Console.Write("Vagas");
        }

        // Exibe uma linha do relatório de eventos disponíveis.
        public void ShowAvailableEventRow(int row, EventModel ev, int vagas)
        {
            string name = this.Truncate(ev.Name, RLT_COL_CPF - RLT_COL_EVENT - 1);
            string location = this.Truncate(ev.Location, RLT_COL_NAME - RLT_COL_CPF - 1);

            Console.SetCursorPosition(RLT_COL_CODE, row);
            Console.Write(this.Truncate(ev.Code, RLT_COL_EVENT - RLT_COL_CODE - 1));

            Console.SetCursorPosition(RLT_COL_EVENT, row);
            Console.Write(name);

            Console.SetCursorPosition(RLT_COL_CPF, row);
            Console.Write(location);

            Console.SetCursorPosition(RLT_COL_NAME, row);
            Console.Write(ev.Date.ToString("dd/MM/yyyy"));

            Console.SetCursorPosition(RLT_COL_DATE, row);
            Console.Write(vagas);
        }

        public string AskEventCodeForReport()
        {
            this.MontarMoldura(RLT_COL, RLT_ROW, RLT_CF, RLT_LF);
            this.Centralizar(RLT_COL, RLT_CF, RLT_ROW_TITLE,
                "Participantes por Evento");

            Console.SetCursorPosition(RLT_COL + 2, RLT_ROW_HEADER);
            Console.Write("Código do Evento: ");

            return Console.ReadLine() ?? "";
        }

        // Exibe uma linha do relatório de eventos disponíveis.
        public void ShowParticipantsByEventReportForm(EventModel ev)
        {
            this.MontarMoldura(RLT_COL, RLT_ROW, RLT_CF, RLT_LF);
            this.Centralizar(RLT_COL, RLT_CF, RLT_ROW_TITLE,
                "Participantes: " + ev.Name);

            Console.SetCursorPosition(RLT_COL_CODE, RLT_ROW_HEADER);
            Console.Write("CPF");
            Console.SetCursorPosition(RLT_COL_EVENT, RLT_ROW_HEADER);
            Console.Write("Nome");
            Console.SetCursorPosition(RLT_COL_CPF, RLT_ROW_HEADER);
            Console.Write("E-mail");
            Console.SetCursorPosition(RLT_COL_DATE, RLT_ROW_HEADER);
            Console.Write("Inscrição");
        }

        // Exibe uma linha contendo as informações de um participante inscrito.
        public void ShowParticipantRow(int row, RegistrationModel reg, ParticipantModel participant)
        {
            string name = participant != null ? participant.Name : "(não encontrado)";
            string email = participant != null ? participant.Email : "";

            name = this.Truncate(name, RLT_COL_CPF - RLT_COL_EVENT - 1);
            email = this.Truncate(email, RLT_COL_DATE - RLT_COL_CPF - 1);

            Console.SetCursorPosition(RLT_COL_CODE, row);
            Console.Write(this.Truncate(reg.ParticipantCpf, RLT_COL_EVENT - RLT_COL_CODE - 1));

            Console.SetCursorPosition(RLT_COL_EVENT, row);
            Console.Write(name);

            Console.SetCursorPosition(RLT_COL_CPF, row);
            Console.Write(email);

            Console.SetCursorPosition(RLT_COL_DATE, row);
            Console.Write(reg.RegistrationDate.ToString("dd/MM/yy"));
        }

        public void ShowCenteredMessage(int row, string message)
        {
            this.Centralizar(RLT_COL, RLT_CF, row, message);
        }

        public void WaitReport()
        {
            this.Perguntar("Tecle Enter para voltar: ",
                RLT_ROW_PROMPT, RLT_COL + 1, RLT_CF - 1);
        }

        private string Truncate(string text, int maxLength)
        {
            if (text.Length <= maxLength)
                return text;

            return text.Substring(0, maxLength - 1) + "…";
        }
    }
}