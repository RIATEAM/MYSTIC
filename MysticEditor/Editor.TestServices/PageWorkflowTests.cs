using System;
using System.Text;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Editor.DTO;
using Editor.Services;

namespace Editor.TestServices
{
    /// <summary>
    /// Descrizione del riepilogo per UnitTest1
    /// </summary>
    [TestClass]
    public class PageWorkflowTests
    {
        private PageServices svc;
        private static PageDTO page;

        public PageWorkflowTests()
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
        public void A_CreateValidPage()
        {
            // verifico che una pagina con rif. Contenuto, Titolo pubblico, Struttura, Posizione venga salvata correttamente

            page = new PageDTO();

            page.Contentid = 2;
            page.Publictitle = "nuova pagina";
            page.Structureid = 1;
            page.Position = 1;

            //valorizzo la proprietà ISNEW per forzare la creazione
            page.IsNew = true;

            page = svc.SavePage(page);
            Assert.IsTrue(page.IsPersisted, "la pagina non risulta salvata: isPersisted is False!");
            Assert.IsTrue(page.Pageid > 0, "la pagina non ha un identificativo proprio!");
            Assert.IsNotNull(page.PageelementsList, "il sistema non ha creato gli elementi previsti dalla struttura");
        }
        
        [TestMethod]
        public void B_GetExistingPage()
        {
            // verifico il recupero di una pagina tramite identificativo

            page = svc.GetPage(page.Pageid);

            Assert.IsNotNull(page, "la pagina non è stata trovata!");            
        }

        [TestMethod]
        public void C_UpdateExistingPage()
        {
            // verifico il salvataggio delle modifiche al titolo
            page.Publictitle = "nuova pagina modificata";

            //valorizzo la proprietà ISDIRTY per forzare il salvataggio
            page.Dirty = true;

            int id = page.Pageid;

            page = svc.SavePage(page);
                        
            Assert.AreEqual("nuova pagina modificata", page.Publictitle, "titolo non modificato!");
            Assert.AreEqual(id, page.Pageid, "creata nuova pagina!");
        }

        [TestMethod]
        public void D_ClonePageStructure()
        {
            // verifico la clonazione di una pagina ma non di tutti i suoi valori

            PageDTO newPage = new PageDTO();

            newPage.Publictitle = "copia di " + page.Publictitle;
            newPage.Contentid = page.Contentid;
            newPage.Structureid = page.Structureid;
            newPage.Position = page.Position++;  //lato flex è conveniente aggiungere la nuova pagina come ultima!

            //valorizzo la proprietà ISNEW per forzare la creazione (copia)
            newPage.IsNew = true;

            newPage = svc.SavePage(newPage);

            Assert.AreNotEqual(page, newPage, "la pagina è la stessa!");
            Assert.AreNotEqual(page.Pageid, newPage.Pageid, "stesso ID!");
            Assert.AreEqual(page.Structureid, newPage.Structureid, "struttura diversa!!");
        }

        [TestMethod]
        public void E_DeletePage()
        {
            Boolean result = false;
            result = svc.DeletePage(page);

            Assert.IsTrue(result,"pagina non cancellata!");            
        }

        [TestMethod]
        public void F_GetInvalidPage() 
        {
            // verifico il recupero della pagina appena cancellata tramite identificativo

            page = svc.GetPage(page.Pageid);

            Assert.IsNotNull(page, "la pagina è stata cancellata fisicamente!");
            Assert.AreEqual(99, page.State, "la pagina non è stata cancellata!");
        }
    }
}
