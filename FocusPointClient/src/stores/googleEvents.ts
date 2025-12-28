import { defineStore } from 'pinia'
import axios from 'axios'

export interface CalendarEvent {
  id: string
  title: string
  start: string
  end: string
  allDay: boolean
}

interface GoogleEventsState {
  events: CalendarEvent[]
  loading: boolean
  error: string | null
}

export const useGoogleEventsStore = defineStore('googleEvents', {
  state: (): GoogleEventsState => ({
    events: [],
    loading: false,
    error: null
  }),

  getters: {
    hasEvents: (state) => state.events.length > 0,
    upcomingEvents: (state) => {
      const now = new Date()
      return state.events.filter(event => new Date(event.start) >= now)
    },
    eventsByDate: (state) => {
      const grouped: { [key: string]: CalendarEvent[] } = {}
      state.events.forEach(event => {
        const date = new Date(event.start).toDateString()
        if (!grouped[date]) {
          grouped[date] = []
        }
        grouped[date].push(event)
      })
      return grouped
    }
  },

  actions: {
    async fetchEvents() {
      this.loading = true
      this.error = null

      try {
        const response = await axios.get<CalendarEvent[]>('/api/google/events')
        this.events = response.data
      } catch (error: any) {
        console.error('Failed to fetch Google Calendar events:', error)
        this.error = error.response?.data?.message || error.message || 'Failed to load events'

        // If unauthorized, the user might need to reconnect
        if (error.response?.status === 401) {
          this.error = 'Please connect your Google Calendar account'
        }
      } finally {
        this.loading = false
      }
    },

    clearEvents() {
      this.events = []
      this.error = null
    },

    clearError() {
      this.error = null
    }
  }
})