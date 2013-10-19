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
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D texture;

        Grid grid;

        const int ROW = 14;
        const int COLUMN = 11;

        List<Node> lRaffle;
        List<Node> lPathfinding;
        List<Node> lPath;
        Node[,] matrizNodes;
        Random random = new Random();
        int indexAlvo;

        KeyboardManager keyboard;

        Pathfinding pathfinding;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
        }

        protected override void Initialize()
        {
            this.keyboard = new KeyboardManager();

            this.grid = new Grid(ROW, COLUMN);
            matrizNodes = new Node[ROW, COLUMN];
            General.ROW = ROW;
            General.COLUMN = COLUMN;
            this.lRaffle = new List<Node>();
            this.lPathfinding = new List<Node>();

            this.pathfinding = new General();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            this.texture = Content.Load<Texture2D>(@"Texture\white");
        }

        protected override void UnloadContent()
        {
            this.texture.Dispose();
        }

        protected override void Update(GameTime gameTime)
        {
            this.keyboard.Update();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                this.keyboard.IsKeyUp(Keys.Escape))
            {
                this.Exit();
            }

            if (this.keyboard.IsKeyDown(Keys.S))
            {
                this.lPathfinding.Clear();
                this.indexAlvo = random.Next(0, this.grid.Nodes.Length - 1);
                this.grid.Raffle(ref this.lRaffle, this.grid.Nodes[0], this.grid.Nodes[indexAlvo]);
                Window.Title = "Pathfinding: Raffle";
            }

            if (this.keyboard.IsKeyDown(Keys.P))
            {
                this.matrizNodes = this.grid.matrizNodes;
                var general = (General)this.pathfinding;
                general.Search(this.grid.Nodes[0], this.grid.Nodes[indexAlvo],
                                        ref this.grid, ref this.matrizNodes, ref lPathfinding);
                //lRaffle.Clear();
                Window.Title = "Pathfinding: Search";
            }

            this.keyboard.LateUpdate();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            this.spriteBatch.Begin();

            // grid base
            for (var i = 0; i < this.grid.matrizNodes.GetLength(0); i++)
            {
                for (var j = 0; j < this.grid.matrizNodes.GetLength(1); j++)
                {
                    var index = i * this.grid.matrizNodes.GetLength(1) + j;

                    if (this.grid.Nodes[index].EstadoNode == EstadoNode.nenhum)
                    {
                        this.spriteBatch.Draw(this.texture,
                                              new Rectangle(this.grid.matrizNodes[i,j].X,
                                                            this.grid.matrizNodes[i,j].Y,
                                                            this.grid.matrizNodes[i,j].W,
                                                            this.grid.matrizNodes[i,j].H),
                                              Color.White);
                    }
                    else if (this.grid.Nodes[index].EstadoNode == EstadoNode.origem)
                    {
                        this.spriteBatch.Draw(this.texture,
                                              new Rectangle(this.grid.matrizNodes[i,j].X,
                                                            this.grid.matrizNodes[i,j].Y,
                                                            this.grid.matrizNodes[i,j].W,
                                                            this.grid.matrizNodes[i,j].H),
                                              Color.Blue);
                    }
                    else if (this.grid.Nodes[index].EstadoNode == EstadoNode.alvo)
                    {
                        this.spriteBatch.Draw(this.texture,
                                              new Rectangle(this.grid.matrizNodes[i,j].X,
                                                            this.grid.matrizNodes[i,j].Y,
                                                            this.grid.matrizNodes[i,j].W,
                                                            this.grid.matrizNodes[i,j].H),
                                              Color.Red);
                    }
                }
            }

            // grid sorteado
            foreach (Node node in this.lRaffle)
            {
                this.spriteBatch.Draw(this.texture,
                                      new Rectangle(node.X,
                                                    node.Y,
                                                    node.W,
                                                    node.H),
                                      Color.Green);
            }

            // grid depth
            foreach (Node node in lPathfinding)
            {
                if(node != null)
                this.spriteBatch.Draw(this.texture,
                                      new Rectangle(node.X,
                                                    node.Y,
                                                    node.W,
                                                    node.H),
                                      Color.Orange);
            }

            this.spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
