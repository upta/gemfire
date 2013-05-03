using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using System.IO;
using System.Reflection;

namespace Gemfire.Tests
{
    [TestClass]
    public class ScenarioHandlerTests
    {
        static string testFilePath = Path.Combine( Path.GetTempPath(), "scenarios.json" );

        [ClassInitialize]
        static public void Setup( TestContext context )
        {
            Assembly a = Assembly.GetExecutingAssembly();
            Stream s = a.GetManifestResourceStream( "Gemfire.Tests.scenarios.json" );
            StreamReader sr = new StreamReader( s );
            StreamWriter sw = File.CreateText( testFilePath );

            sw.Write( sr.ReadToEnd() );

            sw.Flush();
            sw.Close();
            sr.Close();
        }

        [ClassCleanup]
        static public void Teardown()
        {
            if ( File.Exists( testFilePath ) )
            {
                File.Delete( testFilePath );
            }
        }


        [TestMethod]
        public void CtorWithPath_AddsScenariosToCollection()
        {
            var handler = new ScenarioHandler( testFilePath );

            var result = handler.GetScenarioNames();

            Assert.AreEqual( 1, result.Count() );
        }

        [TestMethod]
        public void AddScenario_AddsToCollection()
        {
            var name = "scen-name";
            var handler = new ScenarioHandler();

            handler.AddScenario( name, new GameState() );
            
            Assert.IsTrue( handler.GetScenarioNames().Any( a => a == name ) );
        }

        [TestMethod]
        public void AddScenario_DoesntDuplicateOnMultipleCalls()
        {
            var name = "scen-name";
            var handler = new ScenarioHandler();

            handler.AddScenario( name, new GameState() );
            handler.AddScenario( name, new GameState() );

            Assert.AreEqual( 1, handler.GetScenarioNames().Count( a => a == name ) );
        }


        [TestMethod]
        public void CopyGameState_ReturnsCopyOfInitialGameState()
        {
            var name = "scen-name";
            var gameState = this.GetGameState();

            var handler = new ScenarioHandler();

            handler.AddScenario( name, gameState );

            var result = handler.CopyGameState( name );

            Assert.AreNotSame( result, gameState, "Should be a copy, not the stored instance" );
            result.ShouldBeEquivalentTo( gameState );
        }


        [TestMethod]
        public void GetScenarioNames_ReturnsAll()
        {
            var handler = new ScenarioHandler();
            var names = new List<string>() { "s1", "s2", "s3" };

            names.ForEach( a =>
            {
                handler.AddScenario( a, new GameState() );
            } );

            var result = handler.GetScenarioNames();

            CollectionAssert.AreEquivalent( names, result.ToList() );
        }


        [TestMethod]
        public void ValidScenario_ReturnsTrueIfInCollection()
        {
            var name = "scen-name";
            var handler = new ScenarioHandler();

            handler.AddScenario( name, new GameState() );

            var result = handler.ValidScenario( name );

            Assert.IsTrue( result );
        }


        private GameState GetGameState()
        {
            return new GameState
            {
                Id = "gs-id",
                GameId = "game-id",
                Families = new List<Family>()
                {
                    new Family
                    {
                        Id = "f-id",
                        Name = "f-name",
                        Members = new List<Character>()
                        {
                            new Character
                            {
                                Charm = 1,
                                Id = "c-id",
                                Intelligence = 2,
                                Name = "c-name",
                                War = 3
                            }
                        }
                    }
                },
                Turn = 123
            };
        }
    }
}
