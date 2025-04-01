package ro.mpp2024.repository;

import ro.mpp2024.domain.Identifiable;

public interface IRepository<ID, T extends Identifiable<ID>> {
    void add(T elem);
    void update(ID id, T elem);
    void delete(ID id);
    T findOne(ID id);
    Iterable<T> findAll();
}