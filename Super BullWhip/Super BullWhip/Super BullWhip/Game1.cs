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

namespace Super_BullWhip
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        List<Obj> objList;
        public Controls controls;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            controls = new Controls();
            objList = new List<Obj>();
            //graphics.IsFullScreen = true;
            graphics.PreferredBackBufferHeight = Global.Height;
            graphics.PreferredBackBufferWidth = Global.Width;
            graphics.ApplyChanges();
            base.Initialize();
        }
        public  void AddObj(Obj obj)
        {
            objList.Add(obj);
            Console.Write("Obj Added");
        }
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            Camera.init();
            spriteBatch = new SpriteBatch(GraphicsDevice);
            for (int i = 0; i < 2000; i+= 50)
            {
                Obj obj = new Obj(this, 350, -300, -i, LoadTex("Character"));
                //obj.zSpeed = -1;
            }
            for (int i = 2000; i > 0; i -= 50)
            {
                Obj obj = new Obj(this, -600,-300, -i, LoadTex("Character"));
                //Camera.Target = obj;
                obj.alpha = 0.5f;
            }
            for (int i = -2000; i < 2000; i+=292)
            {
                Obj obj = new Obj(this, i, 50, 0, LoadTex("Platform"));
            }
            new Player(this, 100, 100, 0);
            Camera.Target = Global.Player;
            
            //obj.zSpeed = -1f;
            // TODO: use this.Content to load your game content here
        }
        public Texture2D LoadTex(string path)
        {
           return Content.Load<Texture2D>(path);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            //Console.WriteLine(objList.Count);
            controls.update();
            Camera.Update();
            for (int i = 0; i < objList.Count; i++)
            {
                objList[i].earlyUpdate();
            }
            for (int i = 0; i < objList.Count; i++)
            {
                objList[i].Update();
            }
            for (int i = 0; i < objList.Count; i++)
            {
                objList[i].lateUpdate();
            }
            sortArray();
            base.Update(gameTime);
        }
        private void sortArray()
        {
            float max = -1000000000000000;
            float max2 = max;
            try
            {
                for (int i = 1; i < objList.Count; i += 1)
                {

                    {

                        if (i > 0 && objList[i] != null)
                        {
                            //Console.WriteLine("Init: " + objList[i].z);
                            //Console.WriteLine("After: " + objList[i - 1].z);
                            if (objList[i].z < objList[i - 1].z)
                            {
                                //if (i>objList.Count)
                                //Console.WriteLine(i);
                                Obj tem = objList[i];
                                objList[i] = objList[i - 1];
                                objList[i - 1] = tem;
                                i -= 2;
                                //Console.WriteLine("Moved Obj");
                            }




                        }

                    }
                }
                for (int i = 1; i < objList.Count; i += 1)
                {

                    {

                        if (i != 0 && objList[i].z == objList[i - 1].z && objList[i].depth < objList[i - 1].depth)
                        {
                            Obj tem = objList[i];
                            objList[i] = objList[i - 1];
                            objList[i - 1] = tem;
                            i -= 2;
                        }
                    }
                }
            }
            catch (NullReferenceException)
            {

            }


        }
        
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// 
        public void SpriteDraw(Texture2D Tex, Vector2 pos)
        {
            spriteBatch.Draw(Tex, pos, Color.White);
        }
        public void SpriteDraw(Texture2D Tex, Vector2 pos, Color color, Vector2 origin, float rotation, Vector2 scale)
        {
            spriteBatch.Draw(Tex, pos, null, color, rotation, origin, scale, SpriteEffects.None, 0);
        }
        protected override void Draw(GameTime gameTime)
        {
            // TODO: Add your drawing code here
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // before we can begin drawing, we must start the spritebatch
            spriteBatch.Begin();
            for (int i = 0; i < objList.Count; i++)
            {
                // each object calles the SpriteDraw function I made in Game1, which calls spriteBatch.Draw, and passes the relative parameters to it
                objList[i].Draw();
            }
            // we have to end the spritebatch when we're done
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}