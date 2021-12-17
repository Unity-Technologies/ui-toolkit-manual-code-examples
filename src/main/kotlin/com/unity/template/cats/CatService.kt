package com.unity.template.cats

class CatService(private val catRepository: CatRepository) {

    fun list(): List<Kitty> {
        return catRepository.getAll()
    }

    fun add(cat: Kitty): Kitty {
        return catRepository.add(cat)
    }

    fun find(id: Int): Kitty {
        return catRepository.get(id)
    }
}
