using MainApp.Models.GOT;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using System.Net;
using System.Data.Entity;

namespace MainApp.Controllers
{
    public class GOTController : Controller
    {
        CharacterContext characterContext = new CharacterContext();

        #region Character List Plur

        [HttpGet]
        public ViewResult GetCharacters()
        {
            IEnumerable<Character> characters = characterContext.Characters;

            return View("Characters", characters);
        }

        [HttpGet]
        public ViewResult GetCharacterByID(int id)
        {
            Character character = characterContext.Characters.SingleOrDefault(x => x.ID == id);

            return View("_character", character);
        }

        #endregion

        #region Characters

        [HttpGet]
        public ViewResult AllCharacters()
        {
            List<Character> characters = characterContext.Characters.ToList();

            return View(characters);
        }

        #endregion

        #region Character Creation

        [HttpGet]
        [ActionName("CreateCharacter")]
        public ViewResult CreateCharacter_GET()
        {        
            return View("Create");
        }

        #region FormCollection

        //[HttpPost]
        //public RedirectToRouteResult CreateCharacter(FormCollection formCollection)
        //{
        //    Character newCharacter = new Character()
        //    {
        //        Firstname = formCollection["Firstname"],
        //        Lastname = formCollection["Lastname"],
        //        Gender = formCollection["Gender"],
        //        Region = formCollection["Region"]
        //    };

        //    characterContext.Characters.Add(newCharacter);
        //    characterContext.SaveChanges();

        //    return RedirectToAction("AllCharacters");
        //}

        #endregion

        [HttpPost]
        [ActionName("CreateCharacter")]
        public RedirectToRouteResult CreateCharacter_POST()
        {
            Character newCharacter = new Character();
            UpdateModel(newCharacter);

            if (ModelState.IsValid)
            {            
                characterContext.Characters.Add(newCharacter);
                characterContext.SaveChanges();               
            }
            return RedirectToAction("AllCharacters");
        }

        #endregion

        #region Character Edit

        [HttpGet]
        [ActionName("EditCharacter")]
        public ActionResult EditCharacter_GET(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Character character = characterContext.Characters.Find(id);

            if(character == null)
            {
                return HttpNotFound();
            }

            return View("Edit", character);
        }

        //[HttpPost]
        //[ActionName("EditCharacter")]
        //public ActionResult EditCharacter_POST(/*[Bind(Include = "Lastname, Gender, Region")]*/ Character characterToEdit)
        //{
            
        //    //Character characterInDB = characterContext.Characters.Single(x => x.ID == characterToEdit.ID);

        //    //characterInDB.Lastname = characterToEdit.Lastname;
        //    //characterInDB.Gender = characterToEdit.Gender;
        //    //characterInDB.Region = characterToEdit.Region;

        //    //characterContext.Entry(characterInDB).State = EntityState.Modified;
        //    //characterContext.SaveChanges();

        //    return Content(characterToEdit.Firstname.ToString());
        //    //return Content(characterContext.Characters.Count().ToString());
            
        //}

        /*************************************  TryUpdateModel include and exclude  ***************************************/

        [HttpPost]
        [ActionName("EditCharacter")]
        public ActionResult EditCharacter_POST(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Character characterToEdit = characterContext.Characters.Find(id);

            if (TryUpdateModel(characterToEdit, "", new string[] { "Lastname", "Gender", "Region" }))
            {
                characterContext.SaveChanges();
            }

            return RedirectToAction("AllCharacters");
        }

        #endregion

        #region Character Deletion

        [HttpPost]
        public ActionResult DeleteCharacter(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Character result = characterContext.Characters.Single(x => x.ID == id);      
            
            characterContext.Characters.Remove(result);
            characterContext.SaveChanges();

            return RedirectToAction("AllCharacters");          
        }

        #endregion
    }
}