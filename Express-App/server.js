const express = require('express');
const axios = require('axios');

const app = express();
const PORT = process.env.PORT || 3000;

// הוסף את ה-API Key שלך כאן
const API_KEY = 'YOUR_API_KEY_HERE';

app.get('/services', async (req, res) => {
    try {
        const response = await axios.get('https://api.render.com/v1/services', {
            headers: {
                Authorization: `Bearer ${API_KEY}`
            }
        });
        res.json(response.data);
    } catch (error) {
        console.error('Error fetching services:', error);
        res.status(500).json({ error: 'Failed to fetch services' });
    }
});

app.listen(PORT, () => {
    console.log(`Server is running on http://localhost:${PORT}`);
});
