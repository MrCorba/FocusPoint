<template>
    <div class="settings-page">
        <h1>Settings</h1>
        
        <div class="settings-container">
            <div class="setting-item">
                <label for="theme">Theme:</label>
                <select id="theme" v-model="settings.theme">
                    <option value="light">Light</option>
                    <option value="dark">Dark</option>
                </select>
            </div>

            <div class="setting-item">
                <label for="language">Language:</label>
                <select id="language" v-model="settings.language">
                    <option value="en">English</option>
                    <option value="es">Español</option>
                </select>
            </div>

            <div class="setting-item">
                <label for="notifications">
                    <input 
                        id="notifications" 
                        type="checkbox" 
                        v-model="settings.notifications"
                    >
                    Enable Notifications
                </label>
            </div>

            <button @click="saveSettings" class="save-btn">Save Settings</button>
        </div>
    </div>
</template>

<script>
export default {
    name: 'SettingsView',
    data() {
        return {
            settings: {
                theme: 'light',
                language: 'en',
                notifications: true
            }
        };
    },
    methods: {
        saveSettings() {
            console.log('Settings saved:', this.settings);
            // Add API call or localStorage logic here
        }
    }
};
</script>

<style scoped>
.settings-page {
    padding: 20px;
    max-width: 600px;
    margin: 0 auto;
}

.settings-container {
    background: #f5f5f5;
    padding: 20px;
    border-radius: 8px;
}

.setting-item {
    margin-bottom: 20px;
}

label {
    display: block;
    margin-bottom: 8px;
    font-weight: 500;
}

select, input {
    padding: 8px;
    border: 1px solid #ccc;
    border-radius: 4px;
}

.save-btn {
    padding: 10px 20px;
    background: #007bff;
    color: white;
    border: none;
    border-radius: 4px;
    cursor: pointer;
}

.save-btn:hover {
    background: #0056b3;
}
</style>