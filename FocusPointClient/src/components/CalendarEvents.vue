<template>
  <div class="calendar-events">
    <div class="events-header">
      <h2>Calendar Events</h2>
      <button
        @click="fetchEvents"
        :disabled="loading"
        class="refresh-btn"
      >
        {{ loading ? 'Loading...' : 'Refresh' }}
      </button>
    </div>

    <div v-if="error" class="error-message">
      <p>{{ error }}</p>
      <button @click="clearError" class="clear-error-btn">Dismiss</button>
    </div>

    <div v-else-if="loading" class="loading">
      <p>Loading calendar events...</p>
    </div>

    <div v-else-if="!hasEvents" class="no-events">
      <p>No upcoming events found.</p>
      <p class="hint">Make sure your Google Calendar is connected.</p>
    </div>

    <div v-else class="events-list">
      <div
        v-for="(eventsOnDate, date) in eventsByDate"
        :key="date"
        class="date-group"
      >
        <h3 class="date-header">{{ formatDate(date as string) }}</h3>
        <div class="events-on-date">
          <div
            v-for="event in eventsOnDate"
            :key="event.id"
            class="event-item"
          >
            <div class="event-time">
              {{ formatTime(event.start, event.allDay) }}
              <span v-if="!event.allDay"> - {{ formatTime(event.end, false) }}</span>
            </div>
            <div class="event-title">{{ event.title || 'Untitled Event' }}</div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed, onMounted } from 'vue'
import { useGoogleEventsStore } from '../stores/googleEvents'

const store = useGoogleEventsStore()

// Reactive state from store
const loading = computed(() => store.loading)
const error = computed(() => store.error)
const hasEvents = computed(() => store.hasEvents)
const eventsByDate = computed(() => store.eventsByDate)

// Actions
const fetchEvents = () => store.fetchEvents()
const clearError = () => store.clearError()

// Format date for display
const formatDate = (dateString: string) => {
  const date = new Date(dateString)
  const today = new Date()
  const tomorrow = new Date(today)
  tomorrow.setDate(today.getDate() + 1)

  if (date.toDateString() === today.toDateString()) {
    return 'Today'
  } else if (date.toDateString() === tomorrow.toDateString()) {
    return 'Tomorrow'
  } else {
    return date.toLocaleDateString('en-US', {
      weekday: 'long',
      month: 'short',
      day: 'numeric'
    })
  }
}

// Format time for display
const formatTime = (dateTimeString: string, isAllDay: boolean) => {
  if (isAllDay) return 'All day'

  const date = new Date(dateTimeString)
  return date.toLocaleTimeString('en-US', {
    hour: 'numeric',
    minute: '2-digit',
    hour12: true
  })
}

// Fetch events on component mount
onMounted(() => {
  fetchEvents()
})
</script>

<style scoped>
.calendar-events {
  max-width: 600px;
  margin: 0 auto;
  padding: 20px;
}

.events-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 20px;
}

.events-header h2 {
  margin: 0;
  color: #333;
}

.refresh-btn {
  padding: 8px 16px;
  background-color: #4285f4;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-size: 14px;
}

.refresh-btn:hover:not(:disabled) {
  background-color: #3367d6;
}

.refresh-btn:disabled {
  background-color: #ccc;
  cursor: not-allowed;
}

.error-message {
  background-color: #ffebee;
  border: 1px solid #f44336;
  border-radius: 4px;
  padding: 12px;
  margin-bottom: 20px;
}

.error-message p {
  margin: 0 0 8px 0;
  color: #c62828;
}

.clear-error-btn {
  background: none;
  border: 1px solid #f44336;
  color: #f44336;
  padding: 4px 8px;
  border-radius: 4px;
  cursor: pointer;
  font-size: 12px;
}

.clear-error-btn:hover {
  background-color: #f44336;
  color: white;
}

.loading, .no-events {
  text-align: center;
  padding: 40px 20px;
  color: #666;
}

.no-events .hint {
  font-size: 14px;
  margin-top: 8px;
  color: #999;
}

.events-list {
  display: flex;
  flex-direction: column;
  gap: 20px;
}

.date-group {
  border: 1px solid #e0e0e0;
  border-radius: 8px;
  overflow: hidden;
}

.date-header {
  background-color: #f5f5f5;
  margin: 0;
  padding: 12px 16px;
  font-size: 16px;
  font-weight: 600;
  color: #333;
  border-bottom: 1px solid #e0e0e0;
}

.events-on-date {
  padding: 0;
}

.event-item {
  display: flex;
  padding: 12px 16px;
  border-bottom: 1px solid #f0f0f0;
  align-items: flex-start;
}

.event-item:last-child {
  border-bottom: none;
}

.event-time {
  flex: 0 0 120px;
  font-size: 14px;
  color: #666;
  font-weight: 500;
}

.event-title {
  flex: 1;
  font-size: 14px;
  color: #333;
  line-height: 1.4;
}
</style>