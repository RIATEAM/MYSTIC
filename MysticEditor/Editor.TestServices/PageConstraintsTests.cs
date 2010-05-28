using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Editor.Services;
using Editor.DTO;

namespace Editor.TestServices
{
    /// <summary>
    /// Descrizione del riepilogo per PageConstraintsTests
    /// </summary>
    [TestClass]
    public class PageConstraintsTests
    {

        private PageServices svc;
        private PageDTO page;

        public PageConstraintsTests()
        {
            //
            // TODO: aggiungere qui la logica del costruttore
            //
            svc = new PageServices();
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Ottiene o imposta il contesto del test che fornisce
        ///le informazioni e le funzionalità per l'esecuzione del test corrente.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Attributi di test aggiuntivi
        //
        // È possibile utilizzare i seguenti attributi aggiuntivi per la scrittura dei test:
        //
        // Utilizzare ClassInitialize per eseguire il codice prima di eseguire il primo test della classe
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Utilizzare ClassCleanup per eseguire il codice dopo l'esecuzione di tutti i test della classe
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Utilizzare TestInitialize per eseguire il codice prima di eseguire ciascun test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Utilizzare TestCleanup per eseguire il codice dopo l'esecuzione di ciascun test
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void PageConstraintsCompleto() {
            A_CreateInvalidPage();
            B_GetInvalidPage();
        }
        
        public void A_CreateInvalidPage()
        {
            // verifico che una pagina senza valori non può essere salvata

            PageDTO invalidPage = new PageDTO();

            //valorizzo la proprietà ISNEW per forzare la creazione
            invalidPage.IsNew = true;

            invalidPage = svc.SavePage(invalidPage);

            Assert.IsFalse(invalidPage.IsPersisted, "la pagina risulta salvata: isPersisted is True!");
            Assert.AreEqual(0, invalidPage.Pageid, "la pagina ha un identificativo!");
        }


        public void B_GetInvalidPage()
        {
            // verifico il recupero di una pagina tramite identificativo

            PageDTO invalidPage = new PageDTO();

            invalidPage = svc.GetPage(9000);

            Assert.IsNull(invalidPage, "la pagina è stata trovata!");
        }

    }
}
