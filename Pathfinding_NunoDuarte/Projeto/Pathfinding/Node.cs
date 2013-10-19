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
    public class Node
    {
        private bool _isPath = false;
        public bool IsPath{get { return this._isPath; }set { this._isPath = value; }}
        private EstadoNode _estadoNode;
        public EstadoNode  EstadoNode{ get{ return this._estadoNode;}set {this._estadoNode = value;} }
        private int _distanciaPassos = 10000;
        public int DistanciaPassos { get { return this._distanciaPassos; } set { this._distanciaPassos = value; } }

        public Node(int i, int j, int x = 0, int y = 0, int w = 50, int h = 50, Node father = null)
        {
            I = i;
            J = j;

            W = w;
            H = h;

            X = W + I * W + I;
            Y = H + J * H + J;

            Father = father;
        }

        public Node Father { get; set; }
        public int I { get; set; }
        public int J { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int W { get; set; }
        public int H { get; set; } 
    }
    public enum EstadoNode
    {
        nenhum, 
        parede,
        alvo,
        origem
    }
}
