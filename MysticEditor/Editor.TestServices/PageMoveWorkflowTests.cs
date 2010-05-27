using System;
using System.Text;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Editor.DTO;
using Editor.Services;

namespace Editor.TestServices {
    /// <summary>
    /// Descrizione del riepilogo per PageMoveWorkflowTests
    /// </summary>
    [TestClass]
    public class PageMoveWorkflowTests {

        private PageServices svc;
        private ContentServices csvc;
        private static ContentDTO content;
        private TestContext testContextInstance;

        private static PageDTO page1 = new PageDTO();          // pos1
        private static PageDTO page2 = new PageDTO();          // pos1
        private static PageDTO page3 = new PageDTO();          // pos2
        private static PageDTO page4 = new PageDTO();          // pos3
        private static PageDTO page5 = new PageDTO();          // pos4
        private static PageDTO page6 = new PageDTO();          //pos1
        private static PageDTO page7 = new PageDTO();          //pos2
        private static PageDTO page8 = new PageDTO();          // pos5
        private static PageDTO page9 = new PageDTO();          //pos1
        private static PageDTO page10 = new PageDTO();         //pos2
        private static PageDTO page11 = new PageDTO();         //pos3

        public PageMoveWorkflowTests() {
            //
            // TODO: aggiungere qui la logica del costruttore
            //
            svc = new PageServices();
            csvc = new ContentServices();
        }



        [TestMethod]
        public void A_CreateContent() {

            content = new ContentDTO();
            content.IsNew = true;
            content.Title = "Nuovo Content Move";
            content.Skinid = 1;

            content = csvc.SaveContent(content);

            Assert.IsTrue(content.IsPersisted, "Il content non risulta salvato: isPersisted is False!");
            Assert.IsTrue(content.Contentid > 0, "Il content non ha un identificativo proprio!");

        }
        
        [TestMethod]
        public void B_CreateStructurePage() {

                        
            //Creo le pagine sul db
            page1.Title = page1.Publictitle = "Page1";
            page1.Contentid = content.Contentid;
            page1.Position = 1;
            page1.Structureid = 3;
            page1.IsNew = true;
            page1 = svc.SavePage(page1);
            //Assert Page1
            Assert.IsTrue(page1.IsPersisted, "la Page1 non risulta salvata: isPersisted is False!");
            Assert.IsTrue(page1.Pageid > 0, "la Page1 non ha un identificativo proprio!");
            Assert.IsTrue(page1.Pageid == page1.Parentpageid, "la Page1 non ha il parentpageid uguale al pageid");
            Assert.IsTrue(page1.Position == 1, "la Page1 non ha position = 1");

            //Creo la pagina2 sul db
            page2.Title = page2.Publictitle = "Page2";
            page2.Contentid = content.Contentid;
            page2.Parentpageid = page1.Pageid;
            page2.Position = 1;
            page2.Structureid = 3;
            page2.IsNew = true;
            page2 = svc.SavePage(page2);
            //Assert Page2
            Assert.IsTrue(page2.IsPersisted, "la Page2 non risulta salvata: isPersisted is False!");
            Assert.IsTrue(page2.Pageid > 0, "la Page2 non ha un identificativo proprio!");
            Assert.IsTrue(page1.Pageid == page2.Parentpageid, "la Page2 non è figlia di Page1");
            Assert.IsTrue(page2.Position == 1, "la Page2 non ha position = 1");

            //Creo la pagina3 sul db
            page3.Title = page3.Publictitle = "Page3";
            page3.Contentid = content.Contentid;
            page3.Parentpageid = page1.Pageid;
            page3.Position = 2;
            page3.Structureid = 3;
            page3.IsNew = true;
            page3 = svc.SavePage(page3);
            //Assert Page3
            Assert.IsTrue(page3.IsPersisted, "la Page3 non risulta salvata: isPersisted is False!");
            Assert.IsTrue(page3.Pageid > 0, "la Page3 non ha un identificativo proprio!");
            Assert.IsTrue(page1.Pageid == page3.Parentpageid, "la Page3 non è figlia di Page1");
            Assert.IsTrue(page3.Position == 2, "la Page3 non ha position = 2");

            //Creo la pagina4 sul db
            page4.Title = page4.Publictitle = "Page4";
            page4.Contentid = content.Contentid;
            page4.Parentpageid = page1.Pageid;
            page4.Position = 3;
            page4.Structureid = 3;
            page4.IsNew = true;
            page4 = svc.SavePage(page4);
            //Assert Page4
            Assert.IsTrue(page4.IsPersisted, "la Page4 non risulta salvata: isPersisted is False!");
            Assert.IsTrue(page4.Pageid > 0, "la Page4 non ha un identificativo proprio!");
            Assert.IsTrue(page1.Pageid == page4.Parentpageid, "la Page4 non è figlia di Page1");
            Assert.IsTrue(page4.Position == 3, "la Page4 non ha position = 3");


            //Creo la pagina5 sul db
            page5.Title = page5.Publictitle = "Page5";
            page5.Contentid = content.Contentid;
            page5.Parentpageid = page1.Pageid;
            page5.Position = 4;
            page5.Structureid = 3;
            page5.IsNew = true;
            page5 = svc.SavePage(page5);
            //Assert Page5
            Assert.IsTrue(page5.IsPersisted, "la Page5 non risulta salvata: isPersisted is False!");
            Assert.IsTrue(page5.Pageid > 0, "la Page5 non ha un identificativo proprio!");
            Assert.IsTrue(page1.Pageid == page5.Parentpageid, "la Page5 non è figlia di Page1");
            Assert.IsTrue(page5.Position == 4, "la Page5 non ha position = 4");

            //Creo la pagina6 sul db
            page6.Title = page6.Publictitle = "Page6";
            page6.Contentid = content.Contentid;
            page6.Parentpageid = page5.Pageid;
            page6.Position = 1;
            page6.Structureid = 3;
            page6.IsNew = true;
            page6 = svc.SavePage(page6);
            //Assert Page6
            Assert.IsTrue(page6.IsPersisted, "la Page6 non risulta salvata: isPersisted is False!");
            Assert.IsTrue(page6.Pageid > 0, "la Page6 non ha un identificativo proprio!");
            Assert.IsTrue(page5.Pageid == page6.Parentpageid, "la Page6 non è figlia di Page5");
            Assert.IsTrue(page6.Position == 1, "la Page6 non ha position = 1");

            //Creo la pagina7 sul db
            page7.Title = page7.Publictitle = "Page7";
            page7.Contentid = content.Contentid;
            page7.Parentpageid = page5.Pageid;
            page7.Position = 2;
            page7.Structureid = 3;
            page7.IsNew = true;
            page7 = svc.SavePage(page7);
            //Assert Page7
            Assert.IsTrue(page7.IsPersisted, "la Page7 non risulta salvata: isPersisted is False!");
            Assert.IsTrue(page7.Pageid > 0, "la Page7 non ha un identificativo proprio!");
            Assert.IsTrue(page5.Pageid == page7.Parentpageid, "la Page7 non è figlia di Page5");
            Assert.IsTrue(page7.Position == 2, "la Page7 non ha position = 2");

            //Creo la pagina8 sul db
            page8.Title = page8.Publictitle = "Page8";
            page8.Contentid = content.Contentid;
            page8.Parentpageid = page1.Pageid;
            page8.Position = 5;
            page8.Structureid = 3;
            page8.IsNew = true;
            page8 = svc.SavePage(page8);
            //Assert Page8
            Assert.IsTrue(page8.IsPersisted, "la Page8 non risulta salvata: isPersisted is False!");
            Assert.IsTrue(page8.Pageid > 0, "la Page8 non ha un identificativo proprio!");
            Assert.IsTrue(page1.Pageid == page8.Parentpageid, "la Page8 non è figlia di Page1");
            Assert.IsTrue(page8.Position == 5, "la Page8 non ha position = 5");

            //Creo la pagina9 sul db
            page9.Title = page9.Publictitle = "Page9";
            page9.Contentid = content.Contentid;
            page9.Parentpageid = page8.Pageid;
            page9.Position = 1;
            page9.Structureid = 3;
            page9.IsNew = true;
            page9 = svc.SavePage(page9);
            //Assert Page9
            Assert.IsTrue(page9.IsPersisted, "la Page9 non risulta salvata: isPersisted is False!");
            Assert.IsTrue(page9.Pageid > 0, "la Page9 non ha un identificativo proprio!");
            Assert.IsTrue(page8.Pageid == page9.Parentpageid, "la Page9 non è figlia di Page8");
            Assert.IsTrue(page9.Position == 1, "la Page9 non ha position = 1");

            //Creo la pagina10 sul db
            page10.Title = page10.Publictitle = "Page10";
            page10.Contentid = content.Contentid;
            page10.Parentpageid = page8.Pageid;
            page10.Position = 2;
            page10.Structureid = 3;
            page10.IsNew = true;
            page10 = svc.SavePage(page10);
            //Assert Page10
            Assert.IsTrue(page10.IsPersisted, "la Page10 non risulta salvata: isPersisted is False!");
            Assert.IsTrue(page10.Pageid > 0, "la Page10 non ha un identificativo proprio!");
            Assert.IsTrue(page8.Pageid == page10.Parentpageid, "la Page10 non è figlia di Page8");
            Assert.IsTrue(page10.Position == 2, "la Page10 non ha position = 2");


            //Creo la pagina11 sul db
            page11.Title = page11.Publictitle = "Page11";
            page11.Contentid = content.Contentid;
            page11.Parentpageid = page8.Pageid;
            page11.Position = 3;
            page11.Structureid = 3;
            page11.IsNew = true;
            page11 = svc.SavePage(page11);
            //Assert Page11
            Assert.IsTrue(page11.IsPersisted, "la Page11 non risulta salvata: isPersisted is False!");
            Assert.IsTrue(page11.Pageid > 0, "la Page11 non ha un identificativo proprio!");
            Assert.IsTrue(page8.Pageid == page11.Parentpageid, "la Page11 non è figlia di Page8");
            Assert.IsTrue(page11.Position == 3, "la Page11 non ha position = 3");


        }

        [TestMethod]
        public void C_Move_Page9_to_Page2() {

            page9.Parentpageid = page2.Pageid;
            page9.Position = 1;
            page9.Dirty = true;

            page9 = svc.MovePage(page9);
            //Assert su Oggetto Restituito
            if (page9.Parentpageid == page8.Pageid) {
                Assert.Fail("La Page9 risulta ancora  figlia della Page8");
            }
            Assert.IsTrue(page9.Parentpageid == page2.Pageid, "La Page9 non risulta figlia della Page2");
            Assert.IsTrue(page9.Position == 1, "La Page9 non ha position = 1");

            PageDTO page9DB = new PageDTO();
            page9DB = svc.GetPage(page9.Pageid);
            //Assert su Oggetto Persistito
            if (page9DB.Parentpageid == page8.Pageid) {
                Assert.Fail("Sul DB La Page9 risulta ancora  figlia della Page8");
            }
            Assert.IsTrue(page9DB.Parentpageid == page2.Pageid, "Sul DB La Page9 non risulta figlia della Page2");
            Assert.IsTrue(page9DB.Position == 1, "Sul DB La Page9 non ha position = 1");

            page9 = page9DB;
        }

        [TestMethod]
        public void D_Move_Page10_to_Page1() {

            page10.Parentpageid = page1.Pageid;
            page10.Position = 1;
            page10.Dirty = true;
            page10 = svc.MovePage(page10);

            //Assert su Oggetto Restituito
            if (page10.Parentpageid == page8.Pageid) {
                Assert.Fail("La Page10 risulta ancora  figlia della Page8");
            }
            Assert.IsTrue(page10.Parentpageid == page1.Pageid, "La Page10 non risulta figlia della Page1");
            Assert.IsTrue(page10.Position == 1, "La Page9 non ha position = 1");

            PageDTO page10DB = new PageDTO();
            page10DB = svc.GetPage(page10.Pageid);
            //Assert su Oggetto Persistito
            if (page10DB.Parentpageid == page8.Pageid) {
                Assert.Fail("Sul DB La page10 risulta ancora  figlia della Page8");
            }
            Assert.IsTrue(page10DB.Parentpageid == page1.Pageid, "Sul DB La Page10 non risulta figlia della Page1");
            Assert.IsTrue(page10DB.Position == 1, "Sul DB La Page10 non ha position = 1");
            
            page10 = page10DB;


            //Verifico se le posizioni sono cambiate ai figli del Page1 
            PageDTO page2DB = new PageDTO();
            page2DB = svc.GetPage(page2.Pageid);
            if (page2DB.Position == page2.Position) {
                Assert.Fail("Sul DB la Page2 non ha incrementato la posizione");
            } else
                if (page2DB.Position != (page2.Position + 1)) {
                    Assert.Fail("Sul DB la Page2 non ha posizione " + (page2.Position + 1));
                }

            page2 = page2DB;

            PageDTO page3DB = new PageDTO();
            page3DB = svc.GetPage(page3.Pageid);
            if (page3DB.Position == page3.Position) {
                Assert.Fail("Sul DB la Page3 non ha incrementato la posizione");
            } else
                if (page3DB.Position != (page3.Position + 1)) {
                    Assert.Fail("Sul DB la Page3 non ha posizione " + (page3.Position + 1));
                }

            page3 = page3DB; 
            
            PageDTO page4DB = new PageDTO();
            page4DB = svc.GetPage(page4.Pageid);
            if (page4DB.Position == page4.Position) {
                Assert.Fail("Sul DB la Page4 non ha incrementato la posizione");
            } else
                if (page4DB.Position != (page4.Position + 1)) {
                    Assert.Fail("Sul DB la Page4 non ha posizione " + (page4.Position + 1));
                }

            page4 = page4DB;

            PageDTO page5DB = new PageDTO();
            page5DB = svc.GetPage(page5.Pageid);
            if (page5DB.Position == page5.Position) {
                Assert.Fail("Sul DB la Page5 non ha incrementato la posizione");
            } else
                if (page5DB.Position != (page5.Position + 1)) {
                    Assert.Fail("Sul DB la Page5 non ha posizione " + (page5.Position + 1));
                }

            page5 = page5DB;

            PageDTO page8DB = new PageDTO();
            page8DB = svc.GetPage(page8.Pageid);
            if (page8DB.Position == page8.Position) {
                Assert.Fail("Sul DB la Page8 non ha incrementato la posizione");
            } else
                if (page8DB.Position != (page8.Position + 1)) {
                    Assert.Fail("Sul DB la Page8 non ha posizione " + (page8.Position + 1));
                }

            page8 = page8DB;

        }

        [TestMethod]
        public void E_Move_Page8_to_Page5() {

            //In Coda
            page8.Parentpageid = page5.Pageid;
            page8.Position = 3;
            page8.Dirty = true;

            page8 = svc.MovePage(page8);
            //Assert su Oggetto Restituito
            if (page8.Parentpageid == page1.Pageid) {
                Assert.Fail("La Page8 risulta ancora  figlia della Page1");
            }
            Assert.IsTrue(page8.Parentpageid == page5.Pageid, "La Page8 non risulta figlia della Page5");
            Assert.IsTrue(page8.Position == 3, "La Page8 non ha position = 3");

            //Assert su Oggetto Persistito
            PageDTO page8DB = new PageDTO();
            page8DB = svc.GetPage(page8.Pageid);
            //Assert su Oggetto Persistito
            if (page8DB.Parentpageid == page1.Pageid) {
                Assert.Fail("Sul DB La Page8 risulta ancora  figlia della Page1");
            }
            Assert.IsTrue(page8DB.Parentpageid == page5.Pageid, "Sul DB La Page8 non risulta figlia della Page5");
            Assert.IsTrue(page8DB.Position == 3, "Sul DB La Page8 non ha position = 3");

            page8 = page8DB;


            //Verifico se le posizioni dei figli del Page5 sono cambiate
            PageDTO page6DB = new PageDTO();
            page6DB = svc.GetPage(page6.Pageid);
            if (page6DB.Position != page6.Position) {
                Assert.Fail("Sul DB la Page6 non ha posizione " + page6.Position ) ;
            } 

            page6 = page6DB;

            PageDTO page7DB = new PageDTO();
            page7DB = svc.GetPage(page7.Pageid);
            if (page7DB.Position != page7.Position) {
                Assert.Fail("Sul DB la Page7 non ha posizione " + page7.Position);
            }

            page7 = page7DB; 

        }

        [TestMethod]
        public void F_Move_Page3_to_Page7() {

            page3.Parentpageid = page7.Pageid;
            page3.Position = 1;
            page3.Dirty = true;

            page3 = svc.MovePage(page3);

            //Assert su Oggetto Restituito
            if (page3.Parentpageid == page1.Pageid) {
                Assert.Fail("La Page3 risulta ancora  figlia della Page1");
            }
            Assert.IsTrue(page3.Parentpageid == page7.Pageid, "La Page3 non risulta figlia della Page7");
            Assert.IsTrue(page3.Position == 1, "La Page3 non ha position = 1");

            //Assert su Oggetto Persistito
            PageDTO page3DB = new PageDTO();
            page3DB = svc.GetPage(page3.Pageid);
            //Assert su Oggetto Persistito
            if (page3DB.Parentpageid == page1.Pageid) {
                Assert.Fail("Sul DB La Page3 risulta ancora  figlia della Page1");
            }
            Assert.IsTrue(page3DB.Parentpageid == page7.Pageid, "Sul DB La Page3 non risulta figlia della Page7");
            Assert.IsTrue(page3DB.Position == 1, "Sul DB La Page3 non ha position = 1");

            page3 = page3DB;
        }

        /// <summary>
        ///Ottiene o imposta il contesto del test che fornisce
        ///le informazioni e le funzionalità per l'esecuzione del test corrente.
        ///</summary>
        public TestContext TestContext {
            get {
                return testContextInstance;
            }
            set {
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
    }
}
