using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ToDoListApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public TodoController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: api/todo/get_tasks
        [HttpGet("get_tasks")]
        public JsonResult GetTasks()
        {
            string query = "SELECT * FROM todo";
            DataTable table = new DataTable();
            string SqlDatasource = _configuration.GetConnectionString("mydb");
            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(SqlDatasource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                }
            }
            return new JsonResult(table);
        }

        // POST: api/todo/add_task
        [HttpPost("add_task")]
        public JsonResult AddTask(TodoItem task)
        {
            string query = @"
                INSERT INTO todo (task) 
                VALUES (@task)";
            string SqlDatasource = _configuration.GetConnectionString("mydb");

            using (SqlConnection myCon = new SqlConnection(SqlDatasource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@task", task.Task);
                    myCommand.ExecuteNonQuery();
                }
            }
            return new JsonResult("Task added successfully");
        }

        // PUT: api/todo/update_task
        [HttpPut("update_task")]
        public JsonResult UpdateTask(TodoItem task)
        {
            string query = @"
                UPDATE todo 
                SET task = @task 
                WHERE id = @id";
            string SqlDatasource = _configuration.GetConnectionString("mydb");

            using (SqlConnection myCon = new SqlConnection(SqlDatasource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@id", task.Id);
                    myCommand.Parameters.AddWithValue("@task", task.Task);
                    myCommand.ExecuteNonQuery();
                }
            }
            return new JsonResult("Task updated successfully");
        }

        // DELETE: api/todo/delete_task/{id}
        [HttpDelete("delete_task/{id}")]
        public JsonResult DeleteTask(int id)
        {
            string query = @"
                DELETE FROM todo 
                WHERE id = @id";
            string SqlDatasource = _configuration.GetConnectionString("mydb");

            using (SqlConnection myCon = new SqlConnection(SqlDatasource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@id", id);
                    myCommand.ExecuteNonQuery();
                }
            }
            return new JsonResult("Task deleted successfully");
        }
    }

    // Model class for TodoItem
    public class TodoItem
    {
        public int Id { get; set; }
        public string Task { get; set; } = "";
    }
}
