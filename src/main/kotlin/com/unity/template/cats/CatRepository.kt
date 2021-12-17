package com.unity.template.cats

import org.jetbrains.exposed.sql.ResultRow
import org.jetbrains.exposed.sql.deleteWhere
import org.jetbrains.exposed.sql.insertAndGetId
import org.jetbrains.exposed.sql.select
import org.jetbrains.exposed.sql.selectAll
import org.jetbrains.exposed.sql.transactions.transaction

class CatRepository {

    fun add(meow: Kitty): Kitty {
        val id = transaction {
            Cats.insertAndGetId {
                it[name] = meow.name
                it[color] = meow.color
            }
        }
        return Kitty(id.value, meow.name, meow.color)
    }

    fun get(id: Int): Kitty = transaction {
        Cats.select { Cats.id eq id }
            .mapNotNull { toKitty(it) }
            .single()
    }

    fun getAll(): List<Kitty> = transaction {
        Cats.selectAll().map { toKitty(it) }
    }

    fun remove(id: Int) = transaction {
        Cats.deleteWhere { Cats.id eq id }
    }

    private fun toKitty(row: ResultRow): Kitty =
        Kitty(
            id = row[Cats.id].value,
            name = row[Cats.name],
            color = row[Cats.color]
        )
}
