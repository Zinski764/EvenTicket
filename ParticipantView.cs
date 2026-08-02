using System;
using System.Collections.Generic;

namespace EvenTicket
{
    // Classe responsável pela interface do módulo de Participantes.
    // Concentra toda a entrada e saída de dados referentes aos participantes.
    internal class ParticipantView : Tela
    {
        private int _column, _row, _width, _heigth;
        private List<string> _fields;

        public ParticipantView(ConsoleColor cf, ConsoleColor ct, int col, int row)
            : base(cf, ct)
        {
            this._column = col;
            this._row = row;

            this._fields = new List<string>();
            this._fields.Add("CPF         : ");
            this._fields.Add("Nome        : ");
            this._fields.Add("E-mail      : ");

            this._width = this._fields[0].Length + 2 + 45;
            this._heigth = this._fields.Count + 2 + 1;
        }

        // Exibe o formulário de cadastro e consulta de participantes.
        public void ShowForm()
        {
            this.MontarMoldura(this._column, this._row,
                this._column + this._width, this._row + this._heigth);

            int row = this._row + 1;
            this.Centralizar(this._column, this._column + this._width,
                row, "Cadastro de Participantes");

            row++;
            for (int i = 0; i < this._fields.Count; i++)
            {
                Console.SetCursorPosition(this._column + 1, row);
                Console.Write(this._fields[i]);
                row++;
            }
        }

        // Solicita ao usuário o CPF do participante.
        public string EnterCpf()
        {
            int col = this._column + 1 + this._fields[0].Length;
            int row = this._row + 2;

            Console.SetCursorPosition(col, row);
            return Console.ReadLine() ?? "";
        }

        // Solicita o preenchimento dos dados do participante e
        // retorna um objeto ParticipantModel.
        public ParticipantModel EnterData(string cpf)
        {
            int col = this._column + 1 + this._fields[0].Length;
            int row = this._row + 3;

            this.LimparArea(col, row,
                this._column + this._width - 2,
                row + this._heigth - 5);

            Console.SetCursorPosition(col, row);
            string name = Console.ReadLine() ?? "";

            row++;
            Console.SetCursorPosition(col, row);
            string email = Console.ReadLine() ?? "";

            return new ParticipantModel(cpf, name, email);
        }

        // Exibe na tela as informações do participante.
        public void ShowData(ParticipantModel participant)
        {
            int col = this._column + 1 + this._fields[0].Length;
            int row = this._row + 3;

            Console.SetCursorPosition(col, row);
            Console.Write(participant.Name);

            row++;
            Console.SetCursorPosition(col, row);
            Console.Write(participant.Email);
        }

        // Exibe uma pergunta ao usuário e retorna a resposta digitada.
        public string Ask(string question)
        {
            int colini = this._column + 1;
            int colfin = this._column + this._width - 1;
            int linha = this._row + this._heigth - 1;

            return this.Perguntar(question, linha, colini, colfin);
        }
    }
}