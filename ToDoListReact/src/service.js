import axios from 'axios';

const apiUrl =process.env.REACT_APP_API_URL;
//"http://localhost:5018";
axios.defaults.baseURL = apiUrl;

// הוספת interceptor לטיפול בשגיאות
axios.interceptors.response.use(
  response => response,
  error => {
    console.error('API Error:', error); // רישום השגיאה בלוג
    return Promise.reject(error);
  }
);

export default {
  getTasks: async () => {
    const result = await axios.get(`${apiUrl}/items`)    
    return result.data;
  },

  addTask: async(name) => {
    console.log('addTask', name);
    try {
      const result = await axios.post(`/items`, { name, isComplete: false });
      console.log(result.data.name);
      return result.data;
      
    } catch (error) {
      console.error('Error adding task:', error);
      throw error;
    }
  },

  setCompleted: async (id, name, isComplete) => {
    console.log('setCompleted', { id, name, isComplete });
    const updatedTask = { name, isComplete };
    await axios.put(`/items/${id}`, updatedTask);
    return { id, isComplete };
  },



  // setCompleted: async (id,name ,isComplete) => {
  //   console.log('setCompleted', { id, isComplete,name });
  //   debugger;
    
  //   try {
      
  //     const result = await axios.put(`/items/${id}`, { isComplete });
  //     console.log(result.name);
      
  //     return result.data;
  //   } catch (error) {
  //     console.error('Error updating task:', error);
  //     throw error;
  //   }
  // },

  deleteTask: async (id) => {
    console.log('deleteTask');
    try {
      await axios.delete(`/items/${id}`);
    } catch (error) {
      console.error('Error deleting task:', error);
      throw error;
    }
  }
};
