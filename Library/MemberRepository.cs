using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Library
{
    static internal class MemberRepository
    {
        static internal event Action? ErroOnMemberSaveToDataBase;
        static internal Member CreateMember
            (
            string name,
            string surname,
            DateTime birthday,
            string adress,
            long IIN,
            string phoneNumber,
            byte[] photo,
            string patronymic
            )
        {
            Member member = new Member
                (
                name,
                surname,
                birthday,
                adress,
                IIN,
                phoneNumber,
                photo,
                patronymic
                );
            return member;
        }
        static internal bool AddMemberToDataBase(LibraryContextForEFcore db, Member member)
        {
            db.Add(member);
            int answer = db.SaveChanges();
            if (answer == 1)
            {
                return true;
            }
            else return false;
        }
        static internal bool RemoveMemberFromDataBase(LibraryContextForEFcore db, Member member)
        {
            db.Remove(member);
            int answer = db.SaveChanges();
            if (answer == 1)
            {
                return true;
            }
            else return false;
        }
        static internal void UpdateMemberInDatabase(LibraryContextForEFcore db, Member member)
        {
            db.Update(member);
            db.SaveChanges();
        }
        static internal Member GetMemberByIIN(LibraryContextForEFcore db, Member? member, long IIN)
        {
            member = db.Members.FirstOrDefault(m => m.IIN == IIN);
            return member!;
        }
    }
}
