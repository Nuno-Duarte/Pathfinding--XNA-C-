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
    public class Grid
    {
        public Node[,] matrizNodes;
        public Grid(int row, int column)
        {
            this.random = new Random();

            this.Row = row;
            this.Column = column;
            matrizNodes = new Node[this.Row,this.Column];
            Nodes = new Node[this.Row * this.Column];

            for (var i = 0; i < this.Row; i++)
            {
                for (var j = 0; j < this.Column; j++)
                {
                    Nodes[i * this.Column + j] = new Node(i, j);
                }
            }
            for (int x = 0; x < matrizNodes.GetLength(0); x++)
            {
                for (int y = 0; y < matrizNodes.GetLength(1); y++)
                {
                    var index = x * matrizNodes.GetLength(1) + y;
                    matrizNodes[x, y] = Nodes[index];
                }
            }
        }

        Random random;

        public int Row { get; set; }
        public int Column { get; set; }

        public Node[] Nodes { get; set; }

        public void Raffle(ref List<Node> list, Node origem, Node destino)
        {
            list.Clear();
            for (int i = 0; i < matrizNodes.GetLength(0); i++)
            {
                for (int j = 0; j < matrizNodes.GetLength(1); j++)
                {
                    matrizNodes[i, j].EstadoNode = EstadoNode.nenhum;
                    matrizNodes[i, j].IsPath = false;
                    matrizNodes[i, j].DistanciaPassos = 10000;

                }
            }
            for (int i = 0; i < matrizNodes.GetLength(0); i++)
            {
                for (int j = 0; j < matrizNodes.GetLength(1); j++)
                {

                    matrizNodes[i, j].EstadoNode = EstadoNode.nenhum;

                    if (matrizNodes[i,j] == origem)
                    {
                        matrizNodes[i, j].EstadoNode = EstadoNode.origem;
                    }
                    else if (matrizNodes[i, j] == destino)
                    {
                        matrizNodes[i, j].EstadoNode = EstadoNode.alvo;
                    }
                }
            }
            for (var i = 0; i < matrizNodes.GetLength(0); i++)
            {
                for (var j = 0; j < matrizNodes.GetLength(1); j++)
                {
                    if (this.random.Next(100) < 30)
                    {
                        var index = j * this.Column + i;
                        if (this.matrizNodes[i,j].EstadoNode != EstadoNode.origem &&
                           this.matrizNodes[i, j].EstadoNode != EstadoNode.alvo)
                        {
                            this.matrizNodes[i, j].EstadoNode = EstadoNode.parede;
                            list.Add(this.matrizNodes[i, j]);
                        }
                    }
                }
            }
        }
    }
}
