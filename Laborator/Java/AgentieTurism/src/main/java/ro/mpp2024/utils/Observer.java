package ro.mpp2024.utils;

public interface Observer<E extends Event> {
    void update(E e);

}
