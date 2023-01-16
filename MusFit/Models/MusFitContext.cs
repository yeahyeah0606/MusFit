using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace MusFit.Models
{
    public partial class MusFitContext : DbContext
    {
        public MusFitContext()
        {
        }

        public MusFitContext(DbContextOptions<MusFitContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Class> Classes { get; set; }
        public virtual DbSet<ClassIntroduce> ClassIntroduces { get; set; }
        public virtual DbSet<ClassOrder> ClassOrders { get; set; }
        public virtual DbSet<ClassRecord> ClassRecords { get; set; }
        public virtual DbSet<ClassTime> ClassTimes { get; set; }
        public virtual DbSet<CoachSpecial> CoachSpecials { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<InBody> InBodies { get; set; }
        public virtual DbSet<KnowledgeColumn> KnowledgeColumns { get; set; }
        public virtual DbSet<LessionCategory> LessionCategories { get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<RoomState> RoomStates { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Term> Terms { get; set; }
        public virtual DbSet<VwCoachSchedule> VwCoachSchedules { get; set; }
        public virtual DbSet<VwInBody> VwInBodies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Class>(entity =>
            {
                entity.HasKey(e => e.CId);

                entity.ToTable("Class");

                entity.Property(e => e.CId).HasColumnName("cID");

                entity.Property(e => e.CActual).HasColumnName("cActual");

                entity.Property(e => e.CExpect).HasColumnName("cExpect");

                entity.Property(e => e.CName)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("cName");

                entity.Property(e => e.CNumber)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("cNumber");

                entity.Property(e => e.CTotalLession).HasColumnName("cTotalLession");

                entity.Property(e => e.Cprice).HasColumnName("cprice");

                entity.Property(e => e.EId).HasColumnName("eID");

                entity.Property(e => e.LcId).HasColumnName("lcID");

                entity.Property(e => e.RoomId).HasColumnName("roomID");

                entity.HasOne(d => d.EIdNavigation)
                    .WithMany(p => p.Classes)
                    .HasForeignKey(d => d.EId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Class_Employee");

                entity.HasOne(d => d.Lc)
                    .WithMany(p => p.Classes)
                    .HasForeignKey(d => d.LcId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Class_LessionCategory");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.Classes)
                    .HasForeignKey(d => d.RoomId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Class_Room");
            });

            modelBuilder.Entity<ClassIntroduce>(entity =>
            {
                entity.HasKey(e => e.InId);

                entity.ToTable("ClassIntroduce");

                entity.Property(e => e.InId).HasColumnName("inID");

                entity.Property(e => e.CId).HasColumnName("cID");

                entity.Property(e => e.EId).HasColumnName("eID");

                entity.Property(e => e.InCategory)
                    .IsRequired()
                    .HasMaxLength(5)
                    .HasColumnName("inCategory");

                entity.Property(e => e.InContent)
                    .IsRequired()
                    .HasMaxLength(250)
                    .HasColumnName("inContent");

                entity.Property(e => e.InDate)
                    .HasColumnType("datetime")
                    .HasColumnName("inDate");

                entity.Property(e => e.InPhoto).HasColumnName("inPhoto");

                entity.Property(e => e.InTitle)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("inTitle")
                    .IsFixedLength(true);

                entity.HasOne(d => d.CIdNavigation)
                    .WithMany(p => p.ClassIntroduces)
                    .HasForeignKey(d => d.CId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ClassIntroduce_Class");

                entity.HasOne(d => d.EIdNavigation)
                    .WithMany(p => p.ClassIntroduces)
                    .HasForeignKey(d => d.EId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ClassIntroduce_Employee");
            });

            modelBuilder.Entity<ClassOrder>(entity =>
            {
                entity.HasKey(e => e.OrderId)
                    .HasName("PK_Order");

                entity.ToTable("ClassOrder");

                entity.Property(e => e.OrderId).HasColumnName("orderID");

                entity.Property(e => e.ClassTimeId).HasColumnName("classTimeID");

                entity.Property(e => e.EId).HasColumnName("eID");

                entity.Property(e => e.OrderStatus)
                    .IsRequired()
                    .HasMaxLength(5)
                    .HasColumnName("orderStatus");

                entity.Property(e => e.OrderTime)
                    .HasColumnType("datetime")
                    .HasColumnName("orderTime");

                entity.Property(e => e.SId).HasColumnName("sID");

                entity.HasOne(d => d.ClassTime)
                    .WithMany(p => p.ClassOrders)
                    .HasForeignKey(d => d.ClassTimeId)
                    .HasConstraintName("FK_ClassOrder_ClassTime");

                entity.HasOne(d => d.EIdNavigation)
                    .WithMany(p => p.ClassOrders)
                    .HasForeignKey(d => d.EId)
                    .HasConstraintName("FK_ClassOrder_Employee");

                entity.HasOne(d => d.SIdNavigation)
                    .WithMany(p => p.ClassOrders)
                    .HasForeignKey(d => d.SId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_Student");
            });

            modelBuilder.Entity<ClassRecord>(entity =>
            {
                entity.HasKey(e => e.CrId);

                entity.ToTable("ClassRecord");

                entity.Property(e => e.CrId).HasColumnName("crID");

                entity.Property(e => e.ClassTimeId).HasColumnName("classTimeID");

                entity.Property(e => e.CrAttendance).HasColumnName("crAttendance");

                entity.Property(e => e.CrContent)
                    .HasMaxLength(50)
                    .HasColumnName("crContent");

                entity.Property(e => e.SId).HasColumnName("sID");

                entity.HasOne(d => d.SIdNavigation)
                    .WithMany(p => p.ClassRecords)
                    .HasForeignKey(d => d.SId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ClassRecord_Student");
            });

            modelBuilder.Entity<ClassTime>(entity =>
            {
                entity.ToTable("ClassTime");

                entity.Property(e => e.ClassTimeId).HasColumnName("classTimeID");

                entity.Property(e => e.CId).HasColumnName("cID");

                entity.Property(e => e.CtDate)
                    .HasColumnType("date")
                    .HasColumnName("ctDate");

                entity.Property(e => e.CtLession).HasColumnName("ctLession");

                entity.Property(e => e.TId).HasColumnName("tID");

                entity.Property(e => e.Weekday)
                    .IsRequired()
                    .HasMaxLength(3)
                    .HasColumnName("weekday")
                    .IsFixedLength(true);

                entity.HasOne(d => d.CIdNavigation)
                    .WithMany(p => p.ClassTimes)
                    .HasForeignKey(d => d.CId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ClassTime_Class");

                entity.HasOne(d => d.TIdNavigation)
                    .WithMany(p => p.ClassTimes)
                    .HasForeignKey(d => d.TId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ClassTime_Term");
            });

            modelBuilder.Entity<CoachSpecial>(entity =>
            {
                entity.HasKey(e => e.CsId);

                entity.ToTable("CoachSpecial");

                entity.Property(e => e.CsId).HasColumnName("csID");

                entity.Property(e => e.EId).HasColumnName("eID");

                entity.Property(e => e.LcId).HasColumnName("lcID");

                entity.HasOne(d => d.EIdNavigation)
                    .WithMany(p => p.CoachSpecials)
                    .HasForeignKey(d => d.EId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CoachSpecial_Employee");

                entity.HasOne(d => d.Lc)
                    .WithMany(p => p.CoachSpecials)
                    .HasForeignKey(d => d.LcId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CoachSpecial_LessionCategory");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.EId);

                entity.ToTable("Employee");

                entity.Property(e => e.EId).HasColumnName("eID");

                entity.Property(e => e.EAccount)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("eAccount");

                entity.Property(e => e.EAddress)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("eAddress");

                entity.Property(e => e.EBirth)
                    .HasColumnType("datetime")
                    .HasColumnName("eBirth");

                entity.Property(e => e.EContactor)
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasColumnName("eContactor");

                entity.Property(e => e.EContactorPhone)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("eContactorPhone");

                entity.Property(e => e.EEngName)
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasColumnName("eEngName")
                    .IsFixedLength(true);

                entity.Property(e => e.EEnrollDate)
                    .HasColumnType("datetime")
                    .HasColumnName("eEnrollDate");

                entity.Property(e => e.EExplain)
                    .HasMaxLength(100)
                    .HasColumnName("eExplain");

                entity.Property(e => e.EGender).HasColumnName("eGender");

                entity.Property(e => e.EIdentityCard)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("eIdentityCard");

                entity.Property(e => e.EIsCoach).HasColumnName("eIsCoach");

                entity.Property(e => e.EMail)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("eMail");

                entity.Property(e => e.EName)
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasColumnName("eName");

                entity.Property(e => e.ENumber)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("eNumber")
                    .HasDefaultValueSql("([dbo].[GeteNumber]())");

                entity.Property(e => e.EPassword)
                    .IsRequired()
                    .HasColumnName("ePassword");

                entity.Property(e => e.EPhone)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ePhone");

                entity.Property(e => e.EPhoto).HasColumnName("ePhoto");

                entity.Property(e => e.EResignDate)
                    .HasColumnType("datetime")
                    .HasColumnName("eResignDate");

                entity.Property(e => e.EToken)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("eToken")
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<InBody>(entity =>
            {
                entity.ToTable("InBody");

                entity.Property(e => e.InBodyId).HasColumnName("inBodyID");

                entity.Property(e => e.Age).HasColumnName("age");

                entity.Property(e => e.Bmi).HasColumnName("BMI");

                entity.Property(e => e.Bmilevel).HasColumnName("BMILevel");

                entity.Property(e => e.BmimaxRange).HasColumnName("BMIMaxRange");

                entity.Property(e => e.BmiminRange).HasColumnName("BMIMinRange");

                entity.Property(e => e.Bmr).HasColumnName("BMR");

                entity.Property(e => e.BmrmaxRange).HasColumnName("BMRMaxRange");

                entity.Property(e => e.BmrminRange).HasColumnName("BMRMinRange");

                entity.Property(e => e.BodyFatMass).HasColumnName("bodyFatMass");

                entity.Property(e => e.BodyFatMassLevel).HasColumnName("bodyFatMassLevel");

                entity.Property(e => e.BodyFatMassMaxRange).HasColumnName("bodyFatMassMaxRange");

                entity.Property(e => e.BodyFatMassMinRange).HasColumnName("bodyFatMassMinRange");

                entity.Property(e => e.BoneMineralLevel).HasColumnName("boneMineralLevel");

                entity.Property(e => e.BottomLeftFat).HasColumnName("bottomLeftFat");

                entity.Property(e => e.BottomLeftFatLevel).HasColumnName("bottomLeftFatLevel");

                entity.Property(e => e.BottomLeftFatPercentage).HasColumnName("bottomLeftFatPercentage");

                entity.Property(e => e.BottomLeftMuscle).HasColumnName("bottomLeftMuscle");

                entity.Property(e => e.BottomLeftMuscleLevel).HasColumnName("bottomLeftMuscleLevel");

                entity.Property(e => e.BottomRightFat).HasColumnName("bottomRightFat");

                entity.Property(e => e.BottomRightFatLevel).HasColumnName("bottomRightFatLevel");

                entity.Property(e => e.BottomRightFatPercentage).HasColumnName("bottomRightFatPercentage");

                entity.Property(e => e.BottomRightMuscle).HasColumnName("bottomRightMuscle");

                entity.Property(e => e.BottomRightMuscleLevel).HasColumnName("bottomRightMuscleLevel");

                entity.Property(e => e.CenterFat).HasColumnName("centerFat");

                entity.Property(e => e.CenterFatLevel).HasColumnName("centerFatLevel");

                entity.Property(e => e.CenterFatPercentage).HasColumnName("centerFatPercentage");

                entity.Property(e => e.CenterMuscle).HasColumnName("centerMuscle");

                entity.Property(e => e.CenterMuscleLevel).HasColumnName("centerMuscleLevel");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.EI100kHzBody).HasColumnName("eI100kHzBody");

                entity.Property(e => e.EI100kHzLeftArm).HasColumnName("eI100kHzLeftArm");

                entity.Property(e => e.EI100kHzLeftLeg).HasColumnName("eI100kHzLeftLeg");

                entity.Property(e => e.EI100kHzRightArm).HasColumnName("eI100kHzRightArm");

                entity.Property(e => e.EI100kHzRightLeg).HasColumnName("eI100kHzRightLeg");

                entity.Property(e => e.EI20kHzBody).HasColumnName("eI20kHzBody");

                entity.Property(e => e.EI20kHzLefttArm).HasColumnName("eI20kHzLefttArm");

                entity.Property(e => e.EI20kHzLefttLeg).HasColumnName("eI20kHzLefttLeg");

                entity.Property(e => e.EI20kHzRightArm).HasColumnName("eI20kHzRightArm");

                entity.Property(e => e.EI20kHzRightLeg).HasColumnName("eI20kHzRightLeg");

                entity.Property(e => e.FatControl)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("fatControl");

                entity.Property(e => e.Ffm).HasColumnName("FFM");

                entity.Property(e => e.FfmmaxRange).HasColumnName("FFMMaxRange");

                entity.Property(e => e.FfmminRange).HasColumnName("FFMMinRange");

                entity.Property(e => e.Height).HasColumnName("height");

                entity.Property(e => e.InBodyScore).HasColumnName("inBodyScore");

                entity.Property(e => e.Mineral).HasColumnName("mineral");

                entity.Property(e => e.MineralMaxRange).HasColumnName("mineralMaxRange");

                entity.Property(e => e.MineralMinRange).HasColumnName("mineralMinRange");

                entity.Property(e => e.MuscleControl)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("muscleControl");

                entity.Property(e => e.Pbf).HasColumnName("PBF");

                entity.Property(e => e.Pbflevel).HasColumnName("PBFLevel");

                entity.Property(e => e.PbfmaxRange).HasColumnName("PBFMaxRange");

                entity.Property(e => e.PbfminRange).HasColumnName("PBFMinRange");

                entity.Property(e => e.Protein).HasColumnName("protein");

                entity.Property(e => e.ProteinLevel).HasColumnName("proteinLevel");

                entity.Property(e => e.ProteinMaxRange).HasColumnName("proteinMaxRange");

                entity.Property(e => e.ProteinMinRange).HasColumnName("proteinMinRange");

                entity.Property(e => e.SId).HasColumnName("sID");

                entity.Property(e => e.Smm).HasColumnName("SMM");

                entity.Property(e => e.Smmlevel).HasColumnName("SMMLevel");

                entity.Property(e => e.SmmmaxRange).HasColumnName("SMMMaxRange");

                entity.Property(e => e.SmmminRange).HasColumnName("SMMMinRange");

                entity.Property(e => e.TotalBodyWater).HasColumnName("totalBodyWater");

                entity.Property(e => e.TotalBodyWaterMaxRange).HasColumnName("totalBodyWaterMaxRange");

                entity.Property(e => e.TotalBodyWaterMinRange).HasColumnName("totalBodyWaterMinRange");

                entity.Property(e => e.UpperLeftFat).HasColumnName("upperLeftFat");

                entity.Property(e => e.UpperLeftFatLevel).HasColumnName("upperLeftFatLevel");

                entity.Property(e => e.UpperLeftFatPercentage).HasColumnName("upperLeftFatPercentage");

                entity.Property(e => e.UpperLeftMuscle).HasColumnName("upperLeftMuscle");

                entity.Property(e => e.UpperLeftMuscleLevel).HasColumnName("upperLeftMuscleLevel");

                entity.Property(e => e.UpperRightFat).HasColumnName("upperRightFat");

                entity.Property(e => e.UpperRightFatLevel).HasColumnName("upperRightFatLevel");

                entity.Property(e => e.UpperRightFatPercentage).HasColumnName("upperRightFatPercentage");

                entity.Property(e => e.UpperRightMuscle).HasColumnName("upperRightMuscle");

                entity.Property(e => e.UpperRightMuscleLevel).HasColumnName("upperRightMuscleLevel");

                entity.Property(e => e.Weight).HasColumnName("weight");

                entity.Property(e => e.WeightLevel).HasColumnName("weightLevel");

                entity.Property(e => e.WeightMaxRange).HasColumnName("weightMaxRange");

                entity.Property(e => e.WeightMinRange).HasColumnName("weightMinRange");

                entity.Property(e => e.Whr).HasColumnName("WHR");

                entity.Property(e => e.Whrlevel).HasColumnName("WHRLevel");

                entity.Property(e => e.WhrmaxRange).HasColumnName("WHRMaxRange");

                entity.Property(e => e.WhrminRange).HasColumnName("WHRMinRange");

                entity.HasOne(d => d.SIdNavigation)
                    .WithMany(p => p.InBodies)
                    .HasForeignKey(d => d.SId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InBody_Student");
            });

            modelBuilder.Entity<KnowledgeColumn>(entity =>
            {
                entity.HasKey(e => e.KColumnId);

                entity.ToTable("KnowledgeColumn");

                entity.Property(e => e.KColumnId).HasColumnName("kColumnID");

                entity.Property(e => e.KAuthor)
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasColumnName("kAuthor");

                entity.Property(e => e.KContent)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnName("kContent");

                entity.Property(e => e.KDate)
                    .HasColumnType("datetime")
                    .HasColumnName("kDate");

                entity.Property(e => e.KPhoto1).HasColumnName("kPhoto1");

                entity.Property(e => e.KPhoto2).HasColumnName("kPhoto2");

                entity.Property(e => e.KTitle)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("kTitle");
            });

            modelBuilder.Entity<LessionCategory>(entity =>
            {
                entity.HasKey(e => e.LcId);

                entity.ToTable("LessionCategory");

                entity.Property(e => e.LcId).HasColumnName("lcID");

                entity.Property(e => e.LcAbbrev)
                    .IsRequired()
                    .HasMaxLength(2)
                    .HasColumnName("lcAbbrev")
                    .IsFixedLength(true);

                entity.Property(e => e.LcName)
                    .IsRequired()
                    .HasMaxLength(5)
                    .HasColumnName("lcName");

                entity.Property(e => e.LcThemeColor)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("lcThemeColor");

                entity.Property(e => e.LcType)
                    .IsRequired()
                    .HasMaxLength(5)
                    .HasColumnName("lcType");
            });

            modelBuilder.Entity<News>(entity =>
            {
                entity.HasKey(e => e.NId);

                entity.Property(e => e.NId).HasColumnName("nID");

                entity.Property(e => e.EId).HasColumnName("eID");

                entity.Property(e => e.NCategory)
                    .IsRequired()
                    .HasMaxLength(5)
                    .HasColumnName("nCategory");

                entity.Property(e => e.NContent)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnName("nContent");

                entity.Property(e => e.NPhoto)
                    .HasMaxLength(50)
                    .HasColumnName("nPhoto");

                entity.Property(e => e.NPostDate)
                    .HasColumnType("datetime")
                    .HasColumnName("nPostDate");

                entity.Property(e => e.NTakeDownTime)
                    .HasColumnType("datetime")
                    .HasColumnName("nTakeDownTime");

                entity.Property(e => e.NTitle)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("nTitle");

                entity.HasOne(d => d.EIdNavigation)
                    .WithMany(p => p.News)
                    .HasForeignKey(d => d.EId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_News_Employee");
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.ToTable("Room");

                entity.Property(e => e.RoomId).HasColumnName("roomID");

                entity.Property(e => e.RoomName).HasColumnName("roomName");
            });

            modelBuilder.Entity<RoomState>(entity =>
            {
                entity.HasKey(e => e.RsId);

                entity.ToTable("RoomState");

                entity.Property(e => e.RsId).HasColumnName("rsID");

                entity.Property(e => e.CId).HasColumnName("cID");

                entity.Property(e => e.ClassTimeId).HasColumnName("classTimeID");

                entity.Property(e => e.RoomId).HasColumnName("roomID");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.RoomStates)
                    .HasForeignKey(d => d.RoomId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RoomState_Room");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.SId);

                entity.ToTable("Student");

                entity.Property(e => e.SId).HasColumnName("sID");

                entity.Property(e => e.SAccount)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("sAccount");

                entity.Property(e => e.SAddress)
                    .HasMaxLength(50)
                    .HasColumnName("sAddress");

                entity.Property(e => e.SBirth)
                    .HasColumnType("datetime")
                    .HasColumnName("sBirth");

                entity.Property(e => e.SContactPhone)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("sContactPhone");

                entity.Property(e => e.SContactor)
                    .HasMaxLength(15)
                    .HasColumnName("sContactor");

                entity.Property(e => e.SGender).HasColumnName("sGender");

                entity.Property(e => e.SIsStudentOrNot).HasColumnName("sIsStudentOrNot");

                entity.Property(e => e.SJoinDate)
                    .HasColumnType("datetime")
                    .HasColumnName("sJoinDate");

                entity.Property(e => e.SMail)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("sMail");

                entity.Property(e => e.SName)
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasColumnName("sName");

                entity.Property(e => e.SNumber)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("sNumber")
                    .HasDefaultValueSql("([dbo].[GetsNumber]())");

                entity.Property(e => e.SPassword).HasColumnName("sPassword");

                entity.Property(e => e.SPhone)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("sPhone");

                entity.Property(e => e.SPhoto).HasColumnName("sPhoto");

                entity.Property(e => e.SToken)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("sToken")
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<Term>(entity =>
            {
                entity.HasKey(e => e.TId)
                    .HasName("PK__Term__DC1157076C8C2B51");

                entity.ToTable("Term");

                entity.Property(e => e.TId).HasColumnName("tID");

                entity.Property(e => e.TEndTime).HasColumnName("tEndTime");

                entity.Property(e => e.TStartTime).HasColumnName("tStartTime");
            });

            modelBuilder.Entity<VwCoachSchedule>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_CoachSchedule");

                entity.Property(e => e.ClassEnd)
                    .IsRequired()
                    .HasMaxLength(81)
                    .IsUnicode(false);

                entity.Property(e => e.ClassId)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("ClassID");

                entity.Property(e => e.ClassName)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.ClassStart)
                    .IsRequired()
                    .HasMaxLength(81)
                    .IsUnicode(false);

                entity.Property(e => e.ClassTitle)
                    .IsRequired()
                    .HasMaxLength(5);

                entity.Property(e => e.CoachId).HasColumnName("CoachID");

                entity.Property(e => e.CoachName)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(e => e.Color)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Weekday)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<VwInBody>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_InBody");

                entity.Property(e => e.Age).HasColumnName("age");

                entity.Property(e => e.Bmi).HasColumnName("BMI");

                entity.Property(e => e.Bmilevel).HasColumnName("BMILevel");

                entity.Property(e => e.BmimaxRange).HasColumnName("BMIMaxRange");

                entity.Property(e => e.BmiminRange).HasColumnName("BMIMinRange");

                entity.Property(e => e.Bmr).HasColumnName("BMR");

                entity.Property(e => e.BmrmaxRange).HasColumnName("BMRMaxRange");

                entity.Property(e => e.BmrminRange).HasColumnName("BMRMinRange");

                entity.Property(e => e.BodyFatMass).HasColumnName("bodyFatMass");

                entity.Property(e => e.BodyFatMassLevel).HasColumnName("bodyFatMassLevel");

                entity.Property(e => e.BodyFatMassMaxRange).HasColumnName("bodyFatMassMaxRange");

                entity.Property(e => e.BodyFatMassMinRange).HasColumnName("bodyFatMassMinRange");

                entity.Property(e => e.BoneMineralLevel).HasColumnName("boneMineralLevel");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.FatControl)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("fatControl");

                entity.Property(e => e.Ffm).HasColumnName("FFM");

                entity.Property(e => e.FfmmaxRange).HasColumnName("FFMMaxRange");

                entity.Property(e => e.FfmminRange).HasColumnName("FFMMinRange");

                entity.Property(e => e.Height).HasColumnName("height");

                entity.Property(e => e.InBodyId).HasColumnName("inBodyID");

                entity.Property(e => e.InBodyScore).HasColumnName("inBodyScore");

                entity.Property(e => e.Mineral).HasColumnName("mineral");

                entity.Property(e => e.MineralMaxRange).HasColumnName("mineralMaxRange");

                entity.Property(e => e.MineralMinRange).HasColumnName("mineralMinRange");

                entity.Property(e => e.MuscleControl)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("muscleControl");

                entity.Property(e => e.Pbf).HasColumnName("PBF");

                entity.Property(e => e.Pbflevel).HasColumnName("PBFLevel");

                entity.Property(e => e.PbfmaxRange).HasColumnName("PBFMaxRange");

                entity.Property(e => e.PbfminRange).HasColumnName("PBFMinRange");

                entity.Property(e => e.Protein).HasColumnName("protein");

                entity.Property(e => e.ProteinLevel).HasColumnName("proteinLevel");

                entity.Property(e => e.ProteinMaxRange).HasColumnName("proteinMaxRange");

                entity.Property(e => e.ProteinMinRange).HasColumnName("proteinMinRange");

                entity.Property(e => e.SId).HasColumnName("sID");

                entity.Property(e => e.SName)
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasColumnName("sName");

                entity.Property(e => e.Smm).HasColumnName("SMM");

                entity.Property(e => e.Smmlevel).HasColumnName("SMMLevel");

                entity.Property(e => e.SmmmaxRange).HasColumnName("SMMMaxRange");

                entity.Property(e => e.SmmminRange).HasColumnName("SMMMinRange");

                entity.Property(e => e.TotalBodyWater).HasColumnName("totalBodyWater");

                entity.Property(e => e.TotalBodyWaterMaxRange).HasColumnName("totalBodyWaterMaxRange");

                entity.Property(e => e.TotalBodyWaterMinRange).HasColumnName("totalBodyWaterMinRange");

                entity.Property(e => e.Weight).HasColumnName("weight");

                entity.Property(e => e.WeightLevel).HasColumnName("weightLevel");

                entity.Property(e => e.WeightMaxRange).HasColumnName("weightMaxRange");

                entity.Property(e => e.WeightMinRange).HasColumnName("weightMinRange");

                entity.Property(e => e.Whr).HasColumnName("WHR");

                entity.Property(e => e.Whrlevel).HasColumnName("WHRLevel");

                entity.Property(e => e.WhrmaxRange).HasColumnName("WHRMaxRange");

                entity.Property(e => e.WhrminRange).HasColumnName("WHRMinRange");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
