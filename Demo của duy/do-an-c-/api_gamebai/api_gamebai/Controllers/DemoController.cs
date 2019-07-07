using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using api_gamebai.Models;

namespace api_gamebai.Controllers
{
    public class DemoController : ApiController
    {
        private DatabaseGameBaiEntities db = new DatabaseGameBaiEntities();

        // GET: api/Demo
        public IQueryable<player_listfriend> Getplayer_listfriend()
        {
            return db.player_listfriend;
        }

        // GET: api/Demo/5
        [ResponseType(typeof(player_listfriend))]
        public async Task<IHttpActionResult> Getplayer_listfriend(int id)
        {
            player_listfriend player_listfriend = await db.player_listfriend.FindAsync(id);
            if (player_listfriend == null)
            {
                return NotFound();
            }

            return Ok(player_listfriend);
        }

        // PUT: api/Demo/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Putplayer_listfriend(int id, player_listfriend player_listfriend)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != player_listfriend.id)
            {
                return BadRequest();
            }

            db.Entry(player_listfriend).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!player_listfriendExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Demo
        [ResponseType(typeof(player_listfriend))]
        public async Task<IHttpActionResult> Postplayer_listfriend(player_listfriend player_listfriend)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.player_listfriend.Add(player_listfriend);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = player_listfriend.id }, player_listfriend);
        }

        // DELETE: api/Demo/5
        [ResponseType(typeof(player_listfriend))]
        public async Task<IHttpActionResult> Deleteplayer_listfriend(int id)
        {
            player_listfriend player_listfriend = await db.player_listfriend.FindAsync(id);
            if (player_listfriend == null)
            {
                return NotFound();
            }

            db.player_listfriend.Remove(player_listfriend);
            await db.SaveChangesAsync();

            return Ok(player_listfriend);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool player_listfriendExists(int id)
        {
            return db.player_listfriend.Count(e => e.id == id) > 0;
        }
    }
}