using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Pathfinding
{
    class General : Pathfinding
    {
        private Point[] movimentos;
        private Node[,] matrizNodes;
        private List<Node> caminho = new List<Node>();
        private static int _ROW;
        private static int _COLUMN;
        public static int ROW { get { return _ROW; } set { _ROW = value; } }
        public static int COLUMN { get { return _COLUMN; } set { _COLUMN = value; } }

        public General()
        {
            movimentos = new Point[]
                {   
                    
                    /*new Point(0, -1),
                    new Point(1, 0),
                    new Point(0, 1),
                    new Point(-1, 0)*/

                    new Point(-1, -1),
                    new Point(0, -1),
                    new Point(1, -1),
                    new Point(1, 0),
                    new Point(1, 1),
                    new Point(0, 1),
                    new Point(-1, 1),
                    new Point(-1, 0)
                };
        }
        public void Search(Node origin, Node goal, ref Grid grid, ref Node[,] matriz, ref List<Node> listaDoCaminho)
        {
            this.matrizNodes = matriz;
            PathfindForOrigin(goal, ref grid);
            PathfindForTarget(origin, ref grid);
            for (int x = 0; x < this.matrizNodes.GetLength(0); x++)
            {
                for (int y = 0; y < this.matrizNodes.GetLength(1); y++)
                {
                    if (this.matrizNodes[x, y].IsPath == true)
                    {
                        if(this.matrizNodes[x,y].EstadoNode != EstadoNode.alvo)
                        listaDoCaminho.Add(this.matrizNodes[x, y]);
                    }
                }
            }
        }

        public void PathfindForOrigin(Node alvo, ref Grid grid)
        {
            Point pontoInicial = ProcurarNode(alvo.EstadoNode);
            int alvoX = pontoInicial.X;
            int alvoY = pontoInicial.Y;
            if (alvoX == -1 || alvoY == -1)
            {
                return;
            }

            matrizNodes[alvoX, alvoY].DistanciaPassos = 0;

            while (true)
            {
                bool verificando = false;

                foreach (Point pontoAtual in TodosNodes())
                {
                    int x = pontoAtual.X;
                    int y = pontoAtual.Y;

                    if (CampoAberto(x,y))
                    {
                        int andandoAqui = matrizNodes[x, y].DistanciaPassos;

                        foreach (Point pontoDeLocomocao in ValidarMovimentos(x, y))
                        {
                            int nX = pontoDeLocomocao.X;
                            int nY = pontoDeLocomocao.Y;
                            int novoAndar = andandoAqui + 1;

                            if (matrizNodes[nX, nY].DistanciaPassos > novoAndar)
                            {
                                matrizNodes[nX, nY].DistanciaPassos = novoAndar;
                                verificando = true;
                            }
                        }
                    }
                }
                if (!verificando)
                {
                    break;
                }
            }
        }
        public void PathfindForTarget(Node alvo, ref Grid grid)
        {
            Point pontoInicial = ProcurarNode(alvo.EstadoNode);
            int origemX = pontoInicial.X;
            int origemY = pontoInicial.Y;
            if (origemX == -1 && origemY == -1)
            {
                return;
            }

            while (true)
            {

                Point menorPonto = Point.Zero;
                int menor = 10000;

                foreach (Point pontoDeLocomocao in ValidarMovimentos(origemX, origemY))
                {
                    int tempPassos = matrizNodes[pontoDeLocomocao.X, pontoDeLocomocao.Y].DistanciaPassos;
                    if (tempPassos < menor)
                    {
                        menor = tempPassos;
                        menorPonto.X = pontoDeLocomocao.X;
                        menorPonto.Y = pontoDeLocomocao.Y;
                    }
                }
                if (menor != 10000)
                {
                    matrizNodes[menorPonto.X, menorPonto.Y].IsPath = true;
                    origemX = menorPonto.X;
                    origemY = menorPonto.Y;
                }
                else
                {
                    break;
                }

                if (matrizNodes[origemX, origemY].EstadoNode == EstadoNode.alvo)
                {
                    break;
                }
            }
        }

        private Point ProcurarNode(EstadoNode estadoNode)
        {
            foreach (Point point in TodosNodes())
            {
                if (matrizNodes[point.X, point.Y].EstadoNode == estadoNode)
                {
                    return new Point(point.X, point.Y);
                }
            }
            return new Point(-1, -1);
        }
        private static IEnumerable<Point> TodosNodes()
        {
            for (int x = 0; x < ROW; x++)
            {
                for (int y = 0; y < COLUMN; y++)
                {
                    yield return new Point(x, y);
                }
            }
        }

        private IEnumerable<Point> ValidarMovimentos(int x, int y)
        {
            foreach (Point pontoDeLocomocao in movimentos)
            {
                int nX = x + pontoDeLocomocao.X;
                int nY = y + pontoDeLocomocao.Y;

                if (ValidarCoordenadas(nX, nY) &&
                    CampoAberto(nX, nY))
                {
                    yield return new Point(nX, nY);
                }
            }
        }
        private bool ValidarCoordenadas(int x, int y)
        {
            if (x < 0)
            {
                return false;
            }
            if (y < 0)
            {
                return false;
            }
            if (x > ROW - 1)
            {
                return false;
            }
            if (y > COLUMN - 1)
            {
                return false;
            }
            return true;
        }
        private bool CampoAberto(int x, int y)
        {
            switch (matrizNodes[x,y].EstadoNode)
            {
                case EstadoNode.nenhum:
                    return true;
                case EstadoNode.alvo:
                    return true;
                case EstadoNode.origem:
                    return true;
                case EstadoNode.parede:
                default:
                    return false;
            }
        }
    }
}
