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
        private ContentServices csvc;
        private static PageDTO page;
        private static ContentDTO content;
        private TestContext testContextInstance;
        
        
        public PageWorkflowTests()
        {
            //
            // TODO: aggiungere qui la logica del costruttore
            //
            svc = new PageServices();
            csvc = new ContentServices();
           
           
        }

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
        public void A_CreateContent() {
             
            content = new ContentDTO();
            content.IsNew = true;
            content.Title = "Nuovo Content";
            content.Skinid = 1;

            content = csvc.SaveContent(content);

            Assert.IsTrue(content.IsPersisted, "Il content non risulta salvato: isPersisted is False!");
            Assert.IsTrue(content.Contentid > 0, "Il content non ha un identificativo proprio!");
        
        }

        [TestMethod]
        public void B_CreateBasket() {

            PageDTO basket = new PageDTO();

            basket.Title = basket.Publictitle = "Cestino";
            basket.Contentid = content.Contentid;
            basket.State = 99;
            basket.Structureid = 1;
            basket.Position = 1;
            basket.IsNew = true;

            basket = svc.SavePage(basket);

            Assert.IsTrue(basket.IsPersisted, "la pagina Cestino non risulta salvata: isPersisted is False!");
            Assert.IsTrue(basket.Pageid > 0, "la pagina Cestino non ha un identificativo proprio!");
            Assert.IsNotNull(basket.PageelementsList, "il sistema non ha creato gli elementi previsti dalla struttura Cestino");
                              
        
        }

        [TestMethod]
        public void C_CreateValidPage()
        {
            // Verifico che una pagina con rif. Contenuto, Titolo pubblico, Struttura, Posizione venga salvata correttamente

            page = new PageDTO();

            page.Contentid = content.Contentid; 
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
        public void D_GetExistingPage()
        {
            // verifico il recupero di una pagina tramite identificativo

            page = svc.GetPage(page.Pageid);

            Assert.IsNotNull(page, "la pagina non è stata trovata!");            
        }

        [TestMethod]
        public void E_UpdateExistingPage()
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
        public void F_ClonePageStructure()
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
        public void G_ClonePageStructureValue() {
            
            PageDTO newPage = new PageDTO();
            newPage.Publictitle = "copia di " + page.Publictitle;
            newPage.Contentid = page.Contentid;
            newPage.Structureid = page.Structureid;
            newPage.Position = page.Position++;
            
            //valorizzo la proprietà ISNEW per forzare la creazione (copia)
            newPage.IsNew = true;

            // Valorizzo il PageID al Padre da clonare
            newPage.Pageid = page.Pageid;

            newPage = svc.ClonePage(newPage);

            Assert.AreNotEqual(page, newPage, "la pagina è la stessa!");
            Assert.AreNotEqual(page.Pageid, newPage.Pageid, "stesso ID!");
            Assert.AreEqual(page.Structureid, newPage.Structureid, "struttura diversa!!");


        }

        [TestMethod]
        public void H_DeletePage()
        {
            Boolean result = false;
            result = svc.DeletePage(page);

            Assert.IsTrue(result,"pagina non cancellata!");            
        }

        [TestMethod]
        public void I_GetInvalidPage() 
        {
            // verifico il recupero della pagina appena cancellata tramite identificativo

            page = svc.GetPage(page.Pageid);

            PageDTO dad = svc.GetPage(page.Parentpageid);

            Assert.IsNotNull(page, "la pagina è stata cancellata fisicamente!");
            Assert.AreEqual(99, dad.State, "la pagina non è stata cancellata!");
        }
    }
}
