package ro.mpp2024.domain;

public interface Identifiable<ID> {
    void setId(ID id);
    ID getId();
}
