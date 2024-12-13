import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { NgForm } from '@angular/forms';
import { FormsModule } from '@angular/forms'; // Import FormsModule for template-driven forms
import { CommonModule } from '@angular/common'; // Import CommonModule for *ngFor

@Component({
  selector: 'app-root',
  standalone: true,
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  imports: [FormsModule, CommonModule] // Declare FormsModule and CommonModule here
})
export class AppComponent implements OnInit {
  title = 'Todo App';
  APIURL = 'http://localhost:8000/api/todo/';  // Backend API base URL
  tasks: any[] = [];  // Holds the list of tasks
  newTask: string = '';  // Input for new task
  updateTaskId: number | null = null;  // Holds the ID of the task being updated
  updateTaskText: string = '';  // Holds the updated task text

  constructor(private http: HttpClient) {}

  // On component initialization, fetch tasks
  ngOnInit() {
    this.getTasks();
  }

  // Fetch all tasks
  getTasks() {
    this.http.get<any[]>(this.APIURL + 'get_tasks').subscribe(
      (res) => {
        this.tasks = res; // Set the response data to tasks
      },
      (err) => {
        console.error('Error fetching tasks:', err);
      }
    );
  }

  // Add a new task
  addTask(form: NgForm) {
    const newTask = { task: this.newTask };
    this.http.post(this.APIURL + 'add_task', newTask).subscribe(
      (res) => {
        this.newTask = '';  // Clear the input field
        this.getTasks();  // Refresh task list
        form.reset();
      },
      (err) => {
        console.error('Error adding task:', err);
      }
    );
  }

  // Prepare for task update
  startUpdateTask(task: any) {
    this.updateTaskId = task.id; // Set the ID of the task being updated
    this.updateTaskText = task.task;  // Prepopulate the input with existing task
  }

  // Update task
  updateTask() {
    if (this.updateTaskId !== null) {
      const updatedTask = { id: this.updateTaskId, task: this.updateTaskText };
      this.http.put(this.APIURL + 'update_task', updatedTask).subscribe(
        (res) => {
          this.updateTaskId = null;  // Clear the update state
          this.updateTaskText = '';  // Clear the input field
          this.getTasks();  // Refresh task list
        },
        (err) => {
          console.error('Error updating task:', err);
        }
      );
    }
  }

  // Delete task
  deleteTask(id: number) {
    this.http.delete(this.APIURL + 'delete_task/' + id).subscribe(
      (res) => {
        this.getTasks();  // Refresh task list after delete
      },
      (err) => {
        console.error('Error deleting task:', err);
      }
    );
  }
}
