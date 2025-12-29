package org.example.firstms;

import org.springframework.data.repository.CrudRepository;
import org.springframework.data.rest.core.annotation.RepositoryRestResource;

@RepositoryRestResource(path = "candidats")
public interface CandidatRepo extends CrudRepository<Candidat, Long> {
}
