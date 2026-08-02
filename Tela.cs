using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvenTicket
{
    internal class Tela
    {
        //
        // atributos
        //
        private ConsoleColor _corFundo;
        private ConsoleColor _corTexto;

        //
        // métodos
        //

        // método construtor 
        // recebe valores iniciais das cores de fundo e texto
        public Tela(ConsoleColor cf, ConsoleColor ct)
        {
            this._corFundo = cf;
            this._corTexto = ct;
        }

        // método construtor sobrecarregado
        public Tela() { }




        // método PrepararTela
        // limpa a tela e prepara para impressão de dados
        public void PrepararTela(string titulo, int ci, int li, int cf, int lf)
        {
            Console.BackgroundColor = this._corFundo;
            Console.ForegroundColor = this._corTexto;
            Console.Clear();
            this.MontarMoldura(ci, li, cf, lf);
            this.Centralizar(ci, cf, li + 1, titulo);
        }

        // método Centralizar
        // centraliza uma mensagem em determinada linha
        public void Centralizar(int ci, int cf, int linha, string texto)
        {
            int coluna = ci + ((cf - ci - texto.Length) / 2);
            Console.SetCursorPosition(coluna, linha);
            Console.Write(texto);
        }

        // método Perguntar
        // faz uma pergunta ao usuário
        public string Perguntar(string pergunta, int linha, int ci, int cf)
        {
            string resp;
            this.LimparArea(ci, linha, cf, linha);
            Console.SetCursorPosition(ci, linha);
            Console.Write(pergunta);
            resp = Console.ReadLine() ?? ""; // evita erro caso Console.ReadLine() retorne null
            return resp.ToLower();
        }

        // método LimparArea
        // limpa uma área retangular da tela
        public void LimparArea(int ci, int li, int cf, int lf)
        {
            for (int x = ci; x <= cf; x++)
            {
                for (int y = li; y <= lf; y++)
                {
                    Console.SetCursorPosition(x, y);
                    Console.Write(" ");
                }
            }
        }

        // método MontarMoldura
        // desenha uma moldura retangular na tela
        public void MontarMoldura(int ci, int li, int cf, int lf)
        {
            int linha, coluna;

            // pede para limpar a area antes de desenhar a moldura
            this.LimparArea(ci, li, cf, lf);

            // desenha as bordas horizontais
            for (coluna = ci; coluna <= cf; coluna++)
            {
                Console.SetCursorPosition(coluna, li);
                Console.Write('═');

                Console.SetCursorPosition(coluna, lf);
                Console.Write('═');
            }

            // desenha as bordas verticais
            for (linha = li; linha <= lf; linha++)
            {
                Console.SetCursorPosition(ci, linha);
                Console.Write("║");

                Console.SetCursorPosition(cf, linha);
                Console.Write("║");
            }

            // desenha o canto superior esquerdo
            Console.SetCursorPosition(ci, li);
            Console.Write('╔');

            // desenha o canto superior direito
            Console.SetCursorPosition(cf, li);
            Console.Write('╗');

            // desenha o canto inferior esquerdo
            Console.SetCursorPosition(ci, lf);
            Console.Write('╚');

            // desenha o canto inferior direito
            Console.SetCursorPosition(cf, lf);
            Console.Write('╝');
        }

        // método MostrarMenu
        // exibe um menu de opções e retorna a opção escolhida pelo usuário
        public string MostrarMenu(int colini, int linini, List<string> opcoes)
        {
            string op;
            int x;
            int colfin = colini + opcoes[0].Length + 1;
            int linfin = linini + opcoes.Count() + 2;

            this.MontarMoldura(colini, linini, colfin, linfin);
            for (x = 0; x < opcoes.Count(); x++)
            {
                Console.SetCursorPosition(colini + 1, linini + 1 + x);
                Console.Write(opcoes[x]);
            }
            Console.SetCursorPosition(colini + 1, linini + 1 + x);
            Console.Write("Opção : ");
            op = Console.ReadLine();

            return op;
        }
    }
}
