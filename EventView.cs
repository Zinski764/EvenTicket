using System;
using System.Collections.Generic;

namespace EvenTicket
{
    // Classe responsável pela interface do módulo de Eventos.
    // Centraliza toda a interação com o usuário referente ao
    // cadastro, consulta e apresentação dos eventos.
    internal class EventView : Tela

    {// posição inicial da interface e dimaneão da janela
        private int _column, _row, _width, _heigth;

        // lista contendo os rótulos dos campos exibidos
        private List<string> _fields;

        // Exibe o formulário utilizado para cadastro e consulta de eventos.
        public EventView(ConsoleColor cf, ConsoleColor ct, int col, int row)
            : base(cf, ct)
        {
            this._column = col;
            this._row = row;

            this._fields = new List<string>();
            this._fields.Add("Código      : ");
            this._fields.Add("Nome        : ");
            this._fields.Add("Local       : ");
            this._fields.Add("Data        : ");
            this._fields.Add("Capacidade  : ");

            this._width = this._fields[0].Length + 2 + 40;
            this._heigth = this._fields.Count + 2 + 1;
        }

        // Exibe o formulário utilizado para cadastro e consulta de eventos.
        public void ShowForm()
        {
            this.MontarMoldura(this._column, this._row,
                this._column + this._width, this._row + this._heigth);

            int row = this._row + 1;
            this.Centralizar(this._column, this._column + this._width,
                row, "Cadastro de Eventos");

            row++;
            for (int i = 0; i < this._fields.Count; i++)
            {
                Console.SetCursorPosition(this._column + 1, row);
                Console.Write(this._fields[i]);
                row++;
            }
        }

        // Solicita ao usuário o código do evento.
        public string EnterCode()
        {
            int col = this._column + 1 + this._fields[0].Length;
            int row = this._row + 2;

            Console.SetCursorPosition(col, row);
            return Console.ReadLine() ?? "";
        }

        // Solicita o preenchimento dos dados do evento e
        // retorna um objeto EventModel contendo as informações digitadas.
        public EventModel EnterData(string code)
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
            string location = Console.ReadLine() ?? "";

            row++;
            Console.SetCursorPosition(col, row);

            DateTime parsedDate;
            string dateInput = Console.ReadLine() ?? "";

            while (!DateTime.TryParseExact(dateInput, "dd/MM/yyyy",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out parsedDate))
            {
                this.LimparArea(col, row,
                    this._column + this._width - 2, row);

                Console.SetCursorPosition(col, row);
                Console.Write("Data inválida. Use dd/MM/yyyy: ");
                dateInput = Console.ReadLine() ?? "";
            }

            row++;
            Console.SetCursorPosition(col, row);

            int parsedCapacity;
            string capInput = Console.ReadLine() ?? "";

            while (!int.TryParse(capInput, out parsedCapacity) ||
                   parsedCapacity <= 0)
            {
                this.LimparArea(col, row,
                    this._column + this._width - 2, row);

                Console.SetCursorPosition(col, row);
                Console.Write("Capacidade inválida. Digite um número positivo: ");
                capInput = Console.ReadLine() ?? "";
            }

            return new EventModel(code, name, location,
                parsedDate, parsedCapacity);
        }

        // Exibe na tela as informações de um evento.
        public void ShowData(EventModel ev)
        {
            int col = this._column + 1 + this._fields[0].Length;
            int row = this._row + 3;

            Console.SetCursorPosition(col, row);
            Console.Write(ev.Name);

            row++;
            Console.SetCursorPosition(col, row);
            Console.Write(ev.Location);

            row++;
            Console.SetCursorPosition(col, row);
            Console.Write(ev.Date.ToString("dd/MM/yyyy"));

            row++;
            Console.SetCursorPosition(col, row);
            Console.Write(ev.Capacity);
        }

        // Exibe uma pergunta ao usuário e retorna a resposta informada.
        public string Ask(string question)
        {
            int colini = this._column + 1;
            int colfin = this._column + this._width - 1;
            int linha = this._row + this._heigth - 1;

            return this.Perguntar(question, linha, colini, colfin);
        }
    }
}