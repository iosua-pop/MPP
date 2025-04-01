package ro.mpp2024.utils;

public class FlightEvent implements Event {
    public enum EventType { FLIGHTS_UPDATED }

    private final EventType type;

    public FlightEvent(EventType type) {
        this.type = type;
    }

    public EventType getType() {
        return type;
    }
}
